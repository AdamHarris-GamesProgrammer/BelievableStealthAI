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
        State state = child.Update();
        switch (state)
        {
            case State.Running:
                return State.Running;
            case State.Failure:
                return State.Success;
            case State.Success:
                return State.Failure;
        }

        return State.Running;
    }
}