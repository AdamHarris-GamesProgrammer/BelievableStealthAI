using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : ActionNode
{
    protected override void OnStart()
    {
        _blackboard._locomotion.CanMove(true);
        //Sets the destination to the last known player position
        _blackboard._locomotion.SetDestination(_blackboard._agent.LastKnownPlayerPosition);
        _blackboard._locomotion.SetMaxSpeed(_blackboard._chaseSpeed);
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        //Updates the position constantly
        _blackboard._locomotion.SetDestination(_blackboard._agent.LastKnownPlayerPosition);

        //if the remaining distance is less than half a meter
        if (_blackboard._locomotion.GetRemainingDistance() < 1.5f)
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