using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToClosestSide : ActionNode
{
    protected override void OnStart()
    {
        _blackboard._locomotion.CanMove(true);
        _blackboard.closestInvestigationSide = _blackboard._changedObservedObject.GetClosestSide(_blackboard._agent.transform.position);
        _blackboard.furthestInvestigationSide = _blackboard._changedObservedObject.GetOppositeSide(_blackboard.closestInvestigationSide);

        _blackboard._locomotion.SetDestination(_blackboard.closestInvestigationSide.position);

    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(_blackboard._locomotion.GetRemainingDistance() < 0.5f)
        {
            return State.Success;
        }
        return State.Running;
    }
}