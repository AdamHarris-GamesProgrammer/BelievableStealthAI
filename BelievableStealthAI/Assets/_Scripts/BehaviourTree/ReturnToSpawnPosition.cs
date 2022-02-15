using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToSpawnPosition : ActionNode
{
    protected override void OnStart()
    {
        _blackboard._locomotion.SetDestination(_blackboard.spawnPosition);
        _blackboard._locomotion.SetMaxSpeed(_blackboard._walkSpeed);
        
        //TODO: Make AI turn to their original orientation when they get back to it
    }

    protected override void OnStop()
    {
        _blackboard._locomotion.Rotation(true);
    }

    protected override State OnUpdate()
    {
        if(_blackboard._locomotion.GetRemainingDistance() < 1.0)
        {
            _blackboard._locomotion.Rotation(false);
            Quaternion rotation = Quaternion.LookRotation(_blackboard.spawnOrientation, Vector3.up);
            _blackboard._agent.transform.rotation = rotation;
            return State.Success;
        }

        return State.Running;
    }
}