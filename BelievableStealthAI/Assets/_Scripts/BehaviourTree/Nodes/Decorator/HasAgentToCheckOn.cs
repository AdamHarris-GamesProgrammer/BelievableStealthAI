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
        //if the agent has a agent to check on
        if(_blackboard._agent.AgentToCheckOn != null)
        {
            //then execute the children
            return child.Update();
        }

        return State.Failure;
    }
}