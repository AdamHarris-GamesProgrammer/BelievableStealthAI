using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlayerHalfwaySeen : DecoratorNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (_blackboard._agent.HalfwaySeen)
        {
            return child.Update();
        }

        return State.Failure;
    }
}