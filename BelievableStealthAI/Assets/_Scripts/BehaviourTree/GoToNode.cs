using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToNode : ActionNode
{
    Vector3 target;

    protected override void OnStart()
    {
        target = _blackboard.moveToPosition;
        Debug.Log(target);
        _blackboard._locomotion.SetDestination(_blackboard.moveToPosition);
        Debug.Log("GoTo: On Start");
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
