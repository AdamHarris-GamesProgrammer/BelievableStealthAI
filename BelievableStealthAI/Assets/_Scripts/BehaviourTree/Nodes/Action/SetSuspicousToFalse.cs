using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSuspicousToFalse : ActionNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        //Makes the agent no longer suspicious
        _blackboard._agent.Suspicious = false;

        return State.Success;
    }
}