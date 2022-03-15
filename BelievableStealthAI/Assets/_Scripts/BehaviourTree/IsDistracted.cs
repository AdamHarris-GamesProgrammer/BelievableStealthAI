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
        if(_blackboard._agent.IsDistracted)
        {
            return child.Update();
        }

        return State.Failure;
    }
}