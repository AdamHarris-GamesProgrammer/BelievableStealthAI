using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToPlayer : ActionNode
{
    Quaternion targetRot;

    public float _rotationSpeed = 10.0f;


    protected override void OnStart()
    {
        targetRot = _blackboard._player.transform.rotation;


        _blackboard._locomotion.Rotation(false);
    }

    protected override void OnStop()
    {
        _blackboard._locomotion.Rotation(true);
    }

    protected override State OnUpdate()
    {
        Quaternion originalRot = _blackboard._agent.transform.rotation;
        _blackboard._agent.transform.rotation = Quaternion.Lerp(originalRot, targetRot, Time.deltaTime * _rotationSpeed);


        float oY = originalRot.eulerAngles.y;
        float nY = targetRot.eulerAngles.y;

        if (Mathf.Approximately(oY, nY))
        {
            return State.Success;
        }

        return State.Success;

    }
}