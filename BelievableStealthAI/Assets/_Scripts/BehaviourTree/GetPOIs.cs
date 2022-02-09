using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPOIs : ActionNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(_blackboard._agent == null)
        {
             Debug.Log("GetPOIs::Error: Agent is null");
        }
        _blackboard._nearbyPointsOfInterest = _blackboard._agent.CurrentRoom.PointsOfInterest;

        if(_blackboard._nearbyPointsOfInterest.Count == 0)
        {
            return State.Failure;
        }

        return State.Success;
    }
}