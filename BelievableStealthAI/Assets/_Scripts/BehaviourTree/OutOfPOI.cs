using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfPOI : DecoratorNode
{
    protected override void OnStart()
    {
        _blackboard._currentPOI = _blackboard._nearbyPointsOfInterest[0];
        _blackboard._nearbyPointsOfInterest.RemoveAt(0);
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(_blackboard._nearbyPointsOfInterest.Count <= 0)
        {
            return State.Failure;
        }

        State state = child.Update();

        switch (state)
        {
            case State.Running:
                return State.Running;
                break;
            case State.Failure:
                _blackboard._nearbyPointsOfInterest.RemoveAt(0);
                if (_blackboard._nearbyPointsOfInterest.Count <= 0)
                {
                    return State.Failure;
                }
                _blackboard._currentPOI = _blackboard._nearbyPointsOfInterest[0];
                return State.Running;
                break;
            case State.Success:
                _blackboard._nearbyPointsOfInterest.RemoveAt(0);
                return State.Success;
                break;
        }

        return State.Running;
    }
}