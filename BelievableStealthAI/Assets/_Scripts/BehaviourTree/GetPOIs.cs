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
        _blackboard._nearbyPointsOfInterest = _blackboard._agent.GetNearbyPointsOfInterest();

        if(_blackboard._nearbyPointsOfInterest.Count == 0)
        {
            return State.Failure;
        }

        return State.Success;
    }
}