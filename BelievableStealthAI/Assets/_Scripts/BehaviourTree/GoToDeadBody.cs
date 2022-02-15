using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToDeadBody : ActionNode
{
    protected override void OnStart()
    {
        _blackboard._locomotion.SetDestination(_blackboard._agent.DeadAgent.transform.position);      
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(_blackboard._locomotion.GetRemainingDistance() < 2.5f)
        {
            return State.Success;
        }
        return State.Running;
    }
}