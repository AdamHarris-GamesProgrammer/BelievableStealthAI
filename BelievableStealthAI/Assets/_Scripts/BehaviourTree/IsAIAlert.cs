using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsAIAlert : DecoratorNode
{
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if(_blackboard._agent.CurrentlyAlert)
        {
            return child.Update();
        }
        else
        {
            return State.Failure;
        }
    }
}