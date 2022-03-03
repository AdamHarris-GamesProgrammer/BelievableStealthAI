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

        GUILayout.Label("Constructing Rooms");
        if (GUILayout.Button("Perform Room Check on this Room"))
        {
            controller.CreateRoomObjects();
        }

        if(GUILayout.Button("Perform Room Check on all Rooms"))
        {
            controller.PerformAllRooms();
        }

        GUILayout.Label("Clearing Rooms");
        if(GUILayout.Button("Clear Room"))
        {
            controller.ClearRoom();
        }
        if(GUILayout.Button("Clear all Rooms"))
        {
            controller.ClearAllRooms();
        }
    }
}
