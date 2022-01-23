using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsObjectDoor : DecoratorNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (_blackboard._changedObservedObject.Type == ObservableType.Door)
        {
            return child.Update();
        }
        return State.Failure;
    }
}