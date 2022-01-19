using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToSoundLocation : ActionNode
{
    protected override void OnStart()
    {
        _blackboard._locomotion.SetDestination(_blackboard._agent.PointOfSound);
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(_blackboard._locomotion.GetRemainingDistance() < 1.0f)
        {
            return State.Success;
        }

        return State.Running;
    }
}