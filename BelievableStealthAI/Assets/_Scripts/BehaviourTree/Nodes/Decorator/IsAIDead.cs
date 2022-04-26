using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsAIDead : DecoratorNode
{
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (_blackboard._health.IsDead)
        {
            return State.Success;
        }
        else
        {
            //if ai is alive return failure
            return child.Update();
        }
    }
}
