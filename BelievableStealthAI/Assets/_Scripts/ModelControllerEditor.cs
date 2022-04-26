using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//This should only be active in the unity editor. Game will not build without this line
#if UNITY_EDITOR

//Custom editor for the ModelController class
[CustomEditor(typeof(ModelController))]
public class ModelControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //Draws the mesh array list
        DrawDefaultInspector();

        //Gets the target and casts it to a model controller
        ModelController controller = (ModelController)target;

        //Buttons for actions in the editor
        if(GUILayout.Button("Random Model for this AI"))
        {
            controller.SetModel();
        }
        if(GUILayout.Button("Random Model for all AI"))
        {
            controller.SetAllModels();
        }
    }
}
#endif
