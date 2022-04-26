using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsDistracted : DecoratorNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        //if the agent is distracted
        if(_blackboard._agent.IsDistracted)
        {
            //update the child
            return child.Update();
        }

        return State.Failure;
    }
}