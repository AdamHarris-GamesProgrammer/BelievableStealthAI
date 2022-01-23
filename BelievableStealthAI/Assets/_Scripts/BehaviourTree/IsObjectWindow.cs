using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsObjectWindow : DecoratorNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(_blackboard._changedObservedObject.Type == ObservableType.Window)
        {
            return child.Update();
        }
        return State.Failure;
    }
}