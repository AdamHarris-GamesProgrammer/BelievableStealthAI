using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SucceederNode : DecoratorNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if(child.Update() != State.Running)
        {
            return State.Success;
        }
        
        return State.Running;
    }
}
