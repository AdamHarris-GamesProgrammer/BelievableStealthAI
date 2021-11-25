using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSocket : MonoBehaviour
{
    [Header("Bone Settings")]
    public MeshSockets.SocketID socketID;
    [SerializeField] HumanBodyBones bone;


    [Header("Offset Settings")]
    [SerializeField] Vector3 offset;
    [SerializeField] Vector3 rotation;

    Transform attachPoint;

    void Awake()
    {
        //Gets the animator component of the parent
        Animator anim = GetComponentInParent<Animator>();
        //Makes the attach point on joint
        attachPoint = new GameObject("Socket" + socketID).transform;
        //Sets the parent
        attachPoint.SetParent(anim.GetBoneTransform(bone));

        //Sets the position and rotation based off the offset
        attachPoint.localPosition = offset;
        attachPoint.localRotation = Quaternion.Euler(rotation);
    }

   public void Attach(Transform objectTransform)
    {
        //Attaches the passed in object to the attach point
        objectTransform.SetParent(attachPoint, false);
    }
}
