using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHeardToFalse : ActionNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        //Stops the agent from hearing a sound
        _blackboard._agent.NoLongerHearingSound();

        return State.Success;
    }
}