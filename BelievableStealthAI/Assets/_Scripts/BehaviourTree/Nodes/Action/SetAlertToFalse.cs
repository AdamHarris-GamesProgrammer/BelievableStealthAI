using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAlertToFalse : ActionNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        //stops the agent from being currently alert
        _blackboard._agent.CurrentlyAlert = false; ;
        return State.Success;
    }
}