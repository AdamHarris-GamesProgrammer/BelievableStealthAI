using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToLastKnownLocation : ActionNode
{
    protected override void OnStart()
    {
        _blackboard._locomotion.CanMove(true);
        _blackboard._locomotion.SetDestination(_blackboard._agent.LastKnownPlayerPosition);
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (_blackboard._locomotion.GetRemainingDistance() < 6.0f)
        {
            return State.Success;
        }
        return State.Running;
    }
}