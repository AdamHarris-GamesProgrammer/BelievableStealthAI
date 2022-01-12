using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : ActionNode
{
    protected override void OnStart()
    {
        Debug.Log("Chase Start");
        _blackboard._locomotion.SetDestination(_blackboard._agent.LastKnownPlayerPosition);
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        _blackboard._locomotion.SetDestination(_blackboard._agent.LastKnownPlayerPosition);
        if (_blackboard._locomotion.GetRemainingDistance() < 2.0f)
        {
            return State.Success;
        }

        if(!_blackboard._agent.CurrentlySeeingPlayer)
        {
            return State.Failure;
        }

        return State.Running;
    }
}