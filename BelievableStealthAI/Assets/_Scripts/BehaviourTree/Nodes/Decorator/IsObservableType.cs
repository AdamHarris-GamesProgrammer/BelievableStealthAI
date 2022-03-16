using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsObservableType : DecoratorNode
{
    public ObservableType type;

    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (_blackboard._changedObservedObject.Type == type)
        {
            return child.Update();
        }
        return State.Failure;
    }
}