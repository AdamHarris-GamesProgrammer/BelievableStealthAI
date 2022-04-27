using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteOnce : DecoratorNode
{
    bool _activated;

    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        _activated = true;
    }

    protected override State OnUpdate()
    {
        //if the node has been activated before then pass this node
        if (_activated) return State.Success;


        return child.Update();
    }
}