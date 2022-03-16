using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasSeenChangedObject : DecoratorNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if(_blackboard._agent.HasAnObjectchanged)
        {
            return child.Update();
        }
        else
        {
            return State.Failure;
        }
    }
}
