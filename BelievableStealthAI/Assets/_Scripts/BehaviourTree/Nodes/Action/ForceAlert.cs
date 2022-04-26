using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceAlert : ActionNode
{
    public bool _alertAll = false;
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        //if this node is set to alert all
        if (_alertAll)
        {
            //force alert all enemies
            _blackboard._agent.ForceAlertAll();
        }
        else
        {
            //only force alert this agent
            _blackboard._agent.ForceAlert(true);
        }

        _blackboard._agent.DeadAgent = null;
        _blackboard._agent.HasSeenBody = false;

        return State.Success;

    }
}