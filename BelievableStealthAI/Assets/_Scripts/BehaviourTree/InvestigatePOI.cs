using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigatePOI : ActionNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        Debug.Log("Investigating POI: " + _blackboard._currentPOI.name);
        if(_blackboard._currentPOI.Search())
        {
            Debug.Log("Something found");
            _blackboard._nearbyPointsOfInterest.RemoveAt(0);
            if(_blackboard._nearbyPointsOfInterest.Count != 0)
                _blackboard._currentPOI = _blackboard._nearbyPointsOfInterest[0];
            return State.Success;
        }
        else
        {
            Debug.Log("Nothing found");
            _blackboard._nearbyPointsOfInterest.RemoveAt(0);
            if (_blackboard._nearbyPointsOfInterest.Count != 0)
                _blackboard._currentPOI = _blackboard._nearbyPointsOfInterest[0];
            return State.Failure;
        }
    }
}