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
        //if the agent is suspicious
        if(_blackboard._agent.Suspicious)
        {
            //then update the child
            return child.Update();
        }

        return State.Failure;
    }
}