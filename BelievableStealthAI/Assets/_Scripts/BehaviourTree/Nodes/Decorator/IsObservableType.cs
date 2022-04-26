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
        //Checks if the current observable is the required type
        if (_blackboard._changedObservedObject.Type == type)
        {
            return child.Update();
        }
        return State.Failure;
    }
}