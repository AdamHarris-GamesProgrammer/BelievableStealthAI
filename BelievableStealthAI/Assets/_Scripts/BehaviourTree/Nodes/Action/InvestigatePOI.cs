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
        if (_blackboard._agent.StopSearching)
        {
            return State.Failure;
        }

        if(_blackboard._currentPOI.Search())
        {
            if (_blackboard._currentPOI.PlayerInside)
            {
                Debug.Log("Player found in locker");
                _blackboard._agent.Attack(true);
            }
            else if(_blackboard._currentPOI.BodybagInside)
            {
                Debug.Log("Body found");
                //Alerts all AI that a body has been found
                _blackboard._agent.ForceAlertAll();
            }

            return State.Success;
        }
        else
        {
            Debug.Log("Nothing found in POI");
            return State.Failure;
        }
    }
}