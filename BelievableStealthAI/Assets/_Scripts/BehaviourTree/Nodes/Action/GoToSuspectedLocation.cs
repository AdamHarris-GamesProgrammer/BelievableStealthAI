using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToSuspectedLocation : ActionNode
{
    protected override void OnStart()
    {
        _blackboard._locomotion.CanMove(true);
        //Sets the destination of the agent to the suspected sighting location
        _blackboard._locomotion.SetDestination(_blackboard._agent.SuspectedSightingLocation);   
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        //if the agent is within 1.2 metres
        if(_blackboard._locomotion.GetRemainingDistance() < 1.2f)
        {
            return State.Success;
        }

        return State.Running;
    }
}