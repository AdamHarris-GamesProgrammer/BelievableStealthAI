using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSockets : MonoBehaviour
{
    //Holds the types of sockets used in the game
    public enum SocketID
    {
        Spine,
        RightHand,
        RightShoulder
    }

    //A dictionary of the mesh sockets on the AI
    Dictionary<SocketID, MeshSocket> socketMap = new Dictionary<SocketID, MeshSocket>();

    void Awake()
    {
        MeshSocket[] sockets = GetComponentsInChildren<MeshSocket>();

        foreach(var socket in sockets)
        {
            //Adds socket to the dictionary
            socketMap.Add(socket.socketID, socket);
            
        }
    }

    public void Attach(Transform objectTransform, SocketID socketId)
    {
        //Attaches an object to the desired socket
        socketMap[socketId].Attach(objectTransform);
    }
}
