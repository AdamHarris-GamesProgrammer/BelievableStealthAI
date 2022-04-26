using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ModelController : MonoBehaviour
{
    //Holds a set of meshes that the model controller can choose from
    [SerializeField] Mesh[] _meshes;

    //This should only be active in the unity editor. Game will not build without this line
#if UNITY_EDITOR
    [ExecuteInEditMode]
    public void SetAllModels()
    {
        //Loops through each model controller in the scene and calls the set model method
        foreach (ModelController controller in FindObjectsOfType<ModelController>())
        {
            controller.SetModel();
        }
    }


    [ExecuteInEditMode]
    public void SetModel()
    {
        //Safety check
        if(_meshes.Length == 0)
        {
            Debug.LogError("[ERROR: ModelController::SetModel]: Meshes array is empty");
            return;
        }

        //Finds a random index based on the size of the meshes array
        int index = Random.Range(0, _meshes.Length - 1);

        //Gets the renderer for this object
        SkinnedMeshRenderer renderer = GetComponentInChildren<SkinnedMeshRenderer>();

        //Sets it to dirty (editable and saveable)
        EditorUtility.SetDirty(renderer);

        //Sets the mesh to the randomly selected mesh
        renderer.sharedMesh = _meshes[index];

        //Bakes the mesh onto the model. Allows character to be animated after switching meshes.
        renderer.BakeMesh(_meshes[index]);
    }
#endif


}
