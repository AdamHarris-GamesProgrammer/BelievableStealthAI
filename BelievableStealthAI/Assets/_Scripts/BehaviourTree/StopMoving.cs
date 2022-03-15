using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMoving : ActionNode
{
    protected override void OnStart()
    {
        _blackboard._locomotion.CanMove(false);
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}