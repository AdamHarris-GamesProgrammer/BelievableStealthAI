using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToPlayer : ActionNode
{
    //Stores a target rotation
    Quaternion targetRot;

    public float _rotationSpeed = 10.0f;


    protected override void OnStart()
    {
        //Gets the rotation of the player and sets it as our target rotation
        targetRot = _blackboard._player.transform.rotation;

        _blackboard._locomotion.Rotation(false);
    }

    protected override void OnStop()
    {
        _blackboard._locomotion.Rotation(true);
    }

    protected override State OnUpdate()
    {
        //Get the current rotation
        Quaternion originalRot = _blackboard._agent.transform.rotation;
        //lerp between the original and the target rot
        _blackboard._agent.transform.rotation = Quaternion.Lerp(originalRot, targetRot, Time.deltaTime * _rotationSpeed);

        //Get the y values of the original and new rotation
        float oY = originalRot.eulerAngles.y;
        float nY = targetRot.eulerAngles.y;

        float diff = Mathf.Abs(nY - oY);
        if (diff < 1.0f)
        {
            return State.Success;
        }

        return State.Running;

    }
}