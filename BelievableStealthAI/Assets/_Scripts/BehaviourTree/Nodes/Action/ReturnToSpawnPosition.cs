using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToSpawnPosition : ActionNode
{
    protected override void OnStart()
    {
        if (_blackboard._health.IsDead) return;

        //Set the destination and max speed
        _blackboard._locomotion.SetDestination(_blackboard.spawnPosition);
        _blackboard._locomotion.SetMaxSpeed(_blackboard._walkSpeed);
    }

    protected override void OnStop()
    {
        _blackboard._locomotion.Rotation(true);
    }

    protected override State OnUpdate()
    {
        //if the ai is within one metre of there destination
        if(_blackboard._locomotion.GetRemainingDistance() < 1.0)
        {
            _blackboard._locomotion.Rotation(false);
            Quaternion rotation = Quaternion.LookRotation(_blackboard.spawnOrientation, Vector3.up);
            _blackboard._agent.transform.rotation = rotation;

            //Successfully moved
            return State.Success;
        }

        return State.Running;
    }
}