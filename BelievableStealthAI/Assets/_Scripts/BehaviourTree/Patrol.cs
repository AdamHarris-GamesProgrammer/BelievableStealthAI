using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : ActionNode
{
    protected override void OnStart()
    {
        _blackboard._agent.GetNextPatrolPoint();
        _blackboard._locomotion.SetDestination(_blackboard.moveToPosition);
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(_blackboard._locomotion.GetRemainingDistance() < 2.0f)
        {
            _blackboard._agent.GetNextPatrolPoint();
            _blackboard._locomotion.SetDestination(_blackboard.moveToPosition);
        }

        if(_blackboard._agent.CurrentlyAlert || _blackboard._agent.CurrentlyHearingSound || _blackboard._agent.CurrentlySeeingPlayer)
        {
            return State.Failure;
        }

        return State.Running;
    }
}