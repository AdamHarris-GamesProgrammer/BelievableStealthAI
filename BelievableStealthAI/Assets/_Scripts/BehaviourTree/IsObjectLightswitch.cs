using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsObjectLightswitch : DecoratorNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (_blackboard._changedObservedObject.Type == ObservableType.Lightswitch)
        {
            return child.Update();
        }
        return State.Failure;
    }
}