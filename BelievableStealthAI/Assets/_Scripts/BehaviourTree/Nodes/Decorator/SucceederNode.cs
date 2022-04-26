using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SucceederNode : DecoratorNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        //If the child's update does not return running then return success
        if(child.Update() != State.Running)
        {
            return State.Success;
        }
        
        //object is running
        return State.Running;
    }
}
