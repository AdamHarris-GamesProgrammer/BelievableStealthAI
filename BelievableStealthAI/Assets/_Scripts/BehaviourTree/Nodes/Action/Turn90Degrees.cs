using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn90Degrees : ActionNode
{
    Quaternion targetRot;

    protected override void OnStart()
    {
        //gets the current agent rotation
        targetRot = _blackboard._agent.transform.rotation;

        //find the correct rotation by multiplying the target by 90 in y axis.
        targetRot *= Quaternion.Euler(0.0f, 90.0f, 0.0f);
    }

    protected override void OnStop()
    {
        
    }


    protected override State OnUpdate()
    {
        //lerp between the current rot and the target rot
        Quaternion originalRot = _blackboard._agent.transform.rotation;
        _blackboard._agent.transform.rotation = Quaternion.Lerp(originalRot, targetRot, Time.deltaTime * 5.0f);

        //stores the original and new y values
        float oY = originalRot.eulerAngles.y;
        float nY = targetRot.eulerAngles.y;

        //check if the values are approximate
        if(Mathf.Approximately(oY, nY))
        {
            return State.Success;
        }

        return State.Running;
    }
}