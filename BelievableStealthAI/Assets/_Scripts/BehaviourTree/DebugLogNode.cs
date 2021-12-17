using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogNode : ActionNode
{
    public string message;
    protected override void OnStart()
    {
        Debug.Log($"OnStart: {message}");
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}
