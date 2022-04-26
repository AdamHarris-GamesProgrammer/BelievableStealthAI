using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverterNode : DecoratorNode
{
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        //executes the child
        State state = child.Update();

        //Inverts the state that the child returned. 
        switch (state)
        {
            case State.Running: //Running is turned into running
                return State.Running;
            case State.Failure: //Failure is turned into success
                return State.Success;
            case State.Success: //Success is turned into failure
                return State.Failure;
        }

        return State.Running;
    }
}