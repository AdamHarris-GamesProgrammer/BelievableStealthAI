using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPOIs : ActionNode
{
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        _blackboard._agent.CurrentRoom.GetNextPOI(ref _blackboard._currentPOI);
        return State.Success;
    }
}