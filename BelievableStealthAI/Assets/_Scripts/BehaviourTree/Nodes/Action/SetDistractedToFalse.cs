using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDistractedToFalse : ActionNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        //Sets the agent to no longer be distracted
        _blackboard._agent.IsDistracted = false;

        return State.Success;
    }
}