using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsAISuspicous : DecoratorNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(_blackboard._agent.Suspicious)
        {
            return child.Update();
        }

        return State.Running;
    }
}