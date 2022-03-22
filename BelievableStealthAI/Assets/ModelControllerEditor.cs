using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(ModelController))]
public class ModelControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ModelController controller = (ModelController)target;

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
