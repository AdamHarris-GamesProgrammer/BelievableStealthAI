using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToLookPoint : ActionNode
{
    protected override void OnStart()
    {
        _blackboard._locomotion.SetDestination(_blackboard._currentLookPoint.position);
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (_blackboard._locomotion.GetRemainingDistance() < 0.5f)
        {
            return State.Success;
        }

        return State.Running;
    }
}