using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfPOI : DecoratorNode
{
    protected override void OnStart()
    {
        _blackboard._currentPOI = _blackboard._nearbyPointsOfInterest[0];
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        State state = child.Update();
        if(state == State.Failure || state == State.Success)
        {
            if (_blackboard._nearbyPointsOfInterest.Count == 0)
            {
                return State.Success;
            }
        }

        return State.Running;
    }
}