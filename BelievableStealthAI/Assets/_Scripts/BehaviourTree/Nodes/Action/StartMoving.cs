using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMoving : ActionNode
{
    protected override void OnStart()
    {
        //alow movement
        _blackboard._locomotion.CanMove(true);
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}