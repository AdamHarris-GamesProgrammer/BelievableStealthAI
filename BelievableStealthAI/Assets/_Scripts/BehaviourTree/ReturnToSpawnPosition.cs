using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToSpawnPosition : ActionNode
{
    protected override void OnStart()
    {
        _blackboard._locomotion.SetDestination(_blackboard.spawnPosition);
        
        //TODO: Make AI turn to their original orientation when they get back to it
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(_blackboard._locomotion.GetRemainingDistance() < 1.0)
        {
            return State.Success;
        }

        return State.Running;
    }
}