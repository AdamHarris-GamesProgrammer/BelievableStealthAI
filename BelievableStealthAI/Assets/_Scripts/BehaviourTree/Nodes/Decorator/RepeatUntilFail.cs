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
        //Update the child's update
        Node.State childState = child.Update();

        //if the child failed then return sucess
        if(childState == State.Failure)
        {
            return State.Success;
        }
        //if the child does not fail then return running
        else
        {
            return State.Running;
        }
    }
}
