using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithObject : ActionNode
{
    protected override void OnStart()
    {
        _blackboard._locomotion.SetDestination(_blackboard.closestInvestigationSide.position);
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(_blackboard._locomotion.GetRemainingDistance() < 0.5f)
        {
            _blackboard._changedObservedObject.InteractWithObject();
            _blackboard._changedObservedObject.InteractAction();
            return State.Success;
        }

        return State.Running;
    }
}