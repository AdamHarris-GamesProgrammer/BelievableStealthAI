using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAround : ActionNode
{
    Transform agentTransform;

    float lookA = -90.0f;
    float lookB = 90.0f;
    float originalRot;

    protected override void OnStart()
    {
        agentTransform = _blackboard._agent.transform;
        

        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        return State.Running;
    }
}