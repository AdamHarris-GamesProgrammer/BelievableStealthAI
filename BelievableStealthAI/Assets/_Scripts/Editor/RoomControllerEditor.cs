using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomController))]
public class RoomControllerEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RoomController controller = (RoomController)target;

        if(GUILayout.Button("Perform Room Check on this Room"))
        {
            controller.CreateRoomObjects();
        }

        if(GUILayout.Button("Perform Room Check on all Rooms"))
        {
            controller.PerformAllRooms();
        }
    }
}
