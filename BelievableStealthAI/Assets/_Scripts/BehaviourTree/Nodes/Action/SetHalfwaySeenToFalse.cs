using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHalfwaySeenToFalse : ActionNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        //Stops the agent from halfway seeing the player
        _blackboard._agent.HalfwaySeen = false;

        return State.Success;
    }
}