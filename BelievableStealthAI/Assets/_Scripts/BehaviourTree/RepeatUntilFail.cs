using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatUntilFail : DecoratorNode
{
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        Node.State childState = child.Update();

        if(childState == State.Failure)
        {
            Debug.Log("Child has failed");
            return State.Success;
        }
        else
        {
            return State.Running;
        }
    }
}
