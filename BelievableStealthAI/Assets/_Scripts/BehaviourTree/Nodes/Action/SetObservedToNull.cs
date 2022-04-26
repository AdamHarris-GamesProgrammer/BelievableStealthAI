using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObservedToNull : ActionNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        //Set the observed object to null
        _blackboard._changedObservedObject = null;
        _blackboard._agent.InvestigatedObject();
        return State.Success;
    }
}