using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn90Degrees : ActionNode
{

    Quaternion targetRot;

    protected override void OnStart()
    {

        targetRot = _blackboard._agent.transform.rotation;
        targetRot *= Quaternion.Euler(0.0f, 90.0f, 0.0f);

        Debug.Log("Original Y Rot: " + _blackboard._agent.transform.rotation.eulerAngles.y + " New Y Rot: " + targetRot.eulerAngles.y);
    }

    protected override void OnStop()
    {
        
    }


    protected override State OnUpdate()
    {
        Quaternion originalRot = _blackboard._agent.transform.rotation;
        _blackboard._agent.transform.rotation = Quaternion.Lerp(originalRot, targetRot, Time.deltaTime * 5.0f);


        float oY = originalRot.eulerAngles.y;
        float nY = targetRot.eulerAngles.y;

        if(Mathf.Approximately(oY, nY))
        {
            return State.Success;
        }

        return State.Running;
    }
}