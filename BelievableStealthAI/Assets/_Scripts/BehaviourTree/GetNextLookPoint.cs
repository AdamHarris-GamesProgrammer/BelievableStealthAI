using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetNextLookPoint : ActionNode
{
    protected override void OnStart()
    {
        _blackboard._lookPoints.RemoveAt(0);
        _blackboard._locomotion.Rotation(true);

    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(_blackboard._lookPoints.Count == 0)
        {
            return State.Failure;
        }

        _blackboard._currentLookPoint = _blackboard._lookPoints[0];

        return State.Success;
    }
}