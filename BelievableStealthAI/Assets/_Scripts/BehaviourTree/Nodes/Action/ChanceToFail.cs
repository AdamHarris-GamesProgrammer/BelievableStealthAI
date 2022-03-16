using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceToFail : ActionNode
{
    public float _chanceToFail = 0.25f;

    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        //if we are in our fail values then we fail
        if(Random.value < _chanceToFail)
        {
            return State.Failure;
        }

        return State.Success;
    }
}