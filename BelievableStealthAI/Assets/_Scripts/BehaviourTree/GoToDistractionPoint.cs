using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToDistractionPoint : ActionNode
{
    protected override void OnStart()
    {
        _blackboard._locomotion.CanMove(true);
        _blackboard._locomotion.SetDestination(_blackboard._agent.DistractionPoint);
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(_blackboard._locomotion.GetRemainingDistance() < 5.0f)
        {
            return State.Success;
        }

        return State.Running;
    }
}