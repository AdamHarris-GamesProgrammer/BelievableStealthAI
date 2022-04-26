using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatNode : DecoratorNode
{
    //Keep track of the current iterations
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
        //Repeat infinitely
        if (iterations == -1)
        {
            //Execute child
            Node.State childState = child.Update();

            //if the child has finished then increment the iterations
            if (childState != Node.State.Running)
            {
                currentIterations++;
            }

            return State.Running;
        }
        else
        {
            //Do we have remaining iterations
            if (currentIterations < iterations)
            {
                //Execute child
                Node.State childState = child.Update();

                //if child is not running
                if (childState != State.Running)
                {
                    currentIterations++;
                }

                return State.Running;
            }
            else
            {
                //Finished all iterations
                return State.Success;
            }
        }

    }
}
