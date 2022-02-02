using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToLookPoint : ActionNode
{
    protected override void OnStart()
    {
        _blackboard._locomotion.SetDestination(_blackboard._currentLookPoint.transform.position);
        Debug.Log("Going to Location: " + _blackboard._currentLookPoint.name);
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (_blackboard._locomotion.GetRemainingDistance() < 1.5f)
        {
            Debug.Log("Arrived At: " + _blackboard._currentLookPoint.name);
            return State.Success;
        }

        return State.Running;
    }
}