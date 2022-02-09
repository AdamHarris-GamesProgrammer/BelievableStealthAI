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
        Debug.Log("Investigating POI: " + _blackboard._currentPOI.name);
        if(_blackboard._currentPOI.Search())
        {
            Debug.Log("Something found");
            return State.Success;
        }
        else
        {
            Debug.Log("Nothing found");
            return State.Failure;
        }
    }
}