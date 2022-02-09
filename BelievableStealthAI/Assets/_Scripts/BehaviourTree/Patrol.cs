using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : ActionNode
{
    protected override void OnStart()
    {
        _blackboard._locomotion.Rotation(true);
        _blackboard._locomotion.SetMaxSpeed(_blackboard._walkSpeed);
        _blackboard._agent.GetNextPatrolPoint();
        _blackboard._locomotion.SetDestination(_blackboard.moveToPosition);
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(_blackboard._locomotion.GetRemainingDistance() < 0.5f)
        {
            _blackboard._locomotion.SetMaxSpeed(_blackboard._patrolSpeed);
            _blackboard._agent.GetNextPatrolPoint();
            _blackboard._locomotion.SetDestination(_blackboard.moveToPosition);
        }
        return State.Running;
    }
}