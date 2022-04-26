using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithObject : ActionNode
{
    protected override void OnStart()
    {
        //Sets the destination to the closest investigation point 
        _blackboard._locomotion.SetDestination(_blackboard.closestInvestigationSide.position);
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        //if the agent is within half a meter 
        if(_blackboard._locomotion.GetRemainingDistance() < 0.5f)
        {
            //interact with the object
            _blackboard._changedObservedObject.InteractWithObject();
            //close the object
            _blackboard._changedObservedObject.Close();
            return State.Success;
        }

        return State.Running;
    }
}