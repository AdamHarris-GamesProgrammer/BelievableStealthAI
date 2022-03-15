using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToSuspectedLocation : ActionNode
{
    protected override void OnStart()
    {
        _blackboard._locomotion.CanMove(true);
        _blackboard._locomotion.SetDestination(_blackboard._agent.SuspectedSightingLocation);   
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(_blackboard._locomotion.GetRemainingDistance() < 1.2f)
        {
            return State.Success;
        }

        return State.Running;
    }
}