using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasAgentToCheckOn : DecoratorNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(_blackboard._agent.AgentToCheckOn != null)
        {
            return child.Update();
        }

        return State.Failure;
    }
}