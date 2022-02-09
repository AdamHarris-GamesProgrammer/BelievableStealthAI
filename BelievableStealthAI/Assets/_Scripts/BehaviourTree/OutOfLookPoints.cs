using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfLookPoints : DecoratorNode
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
        if (state == State.Failure || state == State.Success)
        {
            if (_blackboard._agent.CurrentRoom.OutOfLookPoints())
            {
                return State.Success;
            }
        }

        return State.Running;
    }
}