using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioAlly : ActionNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        //Call the agents radio ally method
        _blackboard._agent.RadioAllyToCheckOn();

        return State.Success;
    }
}