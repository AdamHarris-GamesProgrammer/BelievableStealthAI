using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatNode : DecoratorNode
{
    public int iterations = 0;
    int currentIterations = 0;

    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (iterations == -1)
        {
            Node.State childState = child.Update();

            if (childState == State.Failure || childState == State.Success)
            {
                currentIterations++;
            }

            return State.Running;
        }
        else
        {
            if (currentIterations < iterations)
            {
                Node.State childState = child.Update();

                if (childState == State.Failure || childState == State.Success)
                {
                    currentIterations++;
                }

                return State.Running;
            }
            else
            {
                return State.Success;
            }
        }

    }
}
