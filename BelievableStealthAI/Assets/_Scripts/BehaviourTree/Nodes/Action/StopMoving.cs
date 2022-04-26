using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMoving : ActionNode
{
    protected override void OnStart()
    {
        //Set can move to false
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