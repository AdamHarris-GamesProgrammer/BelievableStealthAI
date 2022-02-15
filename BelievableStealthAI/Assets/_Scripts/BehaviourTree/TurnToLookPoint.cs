using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToLookPoint : ActionNode
{
    protected override void OnStart()
    {
        _blackboard._locomotion.Rotation(false);
    }

    protected override void OnStop()
    {
        _blackboard._locomotion.Rotation(true);
    }

    protected override State OnUpdate()
    {
        Quaternion rotation = Quaternion.LookRotation(_blackboard._currentLookPoint.forward, Vector3.up);
        _blackboard._agent.transform.rotation = rotation;



        return State.Success;
    }
}