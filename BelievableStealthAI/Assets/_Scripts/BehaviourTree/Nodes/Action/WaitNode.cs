using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitNode : ActionNode
{
    //The duration for this node to execute
    public float duration = 1f;
    float startTime;
    protected override void OnStart()
    {
        //Gets the current time
        startTime = Time.time;
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        //if the current time minus the start time is greater than the duration then the wait is over
        if (Time.time - startTime > duration)
        {
            return State.Success;
        }
        return State.Running;
    }
}
