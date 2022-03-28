using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetNextLookPoint : ActionNode
{
    protected override void OnStart()
    {
        _blackboard._locomotion.Rotation(true);
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (_blackboard._agent.CurrentRoom.GetNextLookPoint(ref _blackboard._currentLookPoint))
        {
            return State.Success;
        }

        return State.Failure;
    }
}