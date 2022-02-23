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
        if(_blackboard._agent.CurrentRoom == null)
        {
            Debug.Log("[ERROR: GetPOIs::OnUpdate]: Current room is null");
            return State.Failure;
        }
        if (_blackboard._agent.CurrentRoom.GetNextPOI(ref _blackboard._currentPOI))
        {
            return State.Failure;
        }
        else
        {
            return State.Success;
        }
    }
}