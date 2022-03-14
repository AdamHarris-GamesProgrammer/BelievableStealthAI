using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ModelController : MonoBehaviour
{
    [SerializeField] Mesh[] _meshes;


    [ExecuteInEditMode]
    public void SetAllModels()
    {
        foreach (ModelController controller in FindObjectsOfType<ModelController>())
        {
            controller.SetModel();
        }
    }


    [ExecuteInEditMode]
    public void SetModel()
    {
        int index = Random.Range(0, _meshes.Length - 1);

        SkinnedMeshRenderer renderer = GetComponentInChildren<SkinnedMeshRenderer>();

        EditorUtility.SetDirty(renderer);
        renderer.sharedMesh = _meshes[index];
        renderer.BakeMesh(_meshes[index]);
        //EditorUtility.ClearDirty(renderer);
    }


}
