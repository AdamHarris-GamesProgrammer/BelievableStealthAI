using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAllyToNull : ActionNode
{
    protected override void OnStart()
    {
        //Sets the agent to check on to null
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