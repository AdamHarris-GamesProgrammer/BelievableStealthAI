using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToNode : ActionNode
{
    Vector3 target;

    protected override void OnStart()
    {
        _blackboard._locomotion.CanMove(true);

        target = _blackboard.moveToPosition;
        _blackboard._locomotion.SetDestination(_blackboard.moveToPosition);
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        //Debug.Log("Current Distance: " + Vector3.SqrMagnitude(_blackboard._agent.transform.position - target));
        if (Vector3.SqrMagnitude(_blackboard._agent.transform.position - target) < 6.0f)
        {
            Debug.Log("Here");
            return State.Success;
        }

        
        return State.Running;
    }
}
