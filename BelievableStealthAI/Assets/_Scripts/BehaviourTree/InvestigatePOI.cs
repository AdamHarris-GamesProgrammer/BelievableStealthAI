using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigatePOI : ActionNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(_blackboard._currentPOI.Search())
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}