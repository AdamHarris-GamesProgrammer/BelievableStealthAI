using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : ActionNode
{
    protected override void OnStart()
    {
        _blackboard._locomotion.Rotation(true);
        _blackboard._locomotion.CanMove(true);
        _blackboard._locomotion.SetMaxSpeed(_blackboard._patrolSpeed);
        _blackboard._agent.GetNextPatrolPoint();
        _blackboard._locomotion.SetDestination(_blackboard.moveToPosition);
    }

    protected override void OnStop()
    {
        _blackboard._locomotion.SetMaxSpeed(_blackboard._walkSpeed);
        
    }

    protected override State OnUpdate()
    {
        if(_blackboard._locomotion.GetRemainingDistance() < 0.5f)
        {
            return State.Success;
            //_blackboard._agent.GetNextPatrolPoint();
            //_blackboard._locomotion.SetDestination(_blackboard.moveToPosition);
        }
        return State.Running;
    }
}