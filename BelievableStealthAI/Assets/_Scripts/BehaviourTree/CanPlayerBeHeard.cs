using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanPlayerBeHeard : DecoratorNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if(_blackboard._agent.CurrentlyHearingSound)
        {
            return child.Update();
        }
        else
        {
            return State.Failure;
        }
    }
}
