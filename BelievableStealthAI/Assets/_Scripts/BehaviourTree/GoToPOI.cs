using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToPOI : ActionNode
{
    protected override void OnStart()
    {
        _blackboard._locomotion.SetDestination(_blackboard._currentPOI.InvestigationPoint.position);
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(_blackboard._locomotion.GetRemainingDistance() < 0.2f)
        {
            return State.Success;
            //TODO: Make AI Look at POI
        }

        return State.Running;
    }
}