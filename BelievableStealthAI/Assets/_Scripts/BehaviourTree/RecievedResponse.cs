using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecievedResponse : DecoratorNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(_blackboard._response)
        {
            return State.Success;
        }
        else
        {
            return child.Update();
        }
    }
}