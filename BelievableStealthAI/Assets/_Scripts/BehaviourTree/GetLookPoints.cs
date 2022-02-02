using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLookPoints : ActionNode
{
    protected override void OnStart()
    {
        _blackboard._lookPoints = _blackboard._agent.CurrentRoom.LookAroundPoints;
        if(_blackboard._lookPoints.Count != 0)
            _blackboard._currentLookPoint = _blackboard._lookPoints[0];
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
        return State.Success;
    }
}