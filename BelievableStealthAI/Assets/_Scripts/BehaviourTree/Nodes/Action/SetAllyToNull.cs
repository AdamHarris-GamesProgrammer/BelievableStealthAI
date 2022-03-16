using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAllyToNull : ActionNode
{
    protected override void OnStart()
    {
        _blackboard._agent.AgentToCheckOn = null;
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}