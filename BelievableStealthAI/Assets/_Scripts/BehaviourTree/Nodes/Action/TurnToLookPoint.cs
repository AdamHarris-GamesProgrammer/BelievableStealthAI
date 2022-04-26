using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToLookPoint : ActionNode
{
    //Stores a target rot
    Quaternion targetRot;

    protected override void OnStart()
    {
        //Gets the rotation of the current look point
        targetRot = _blackboard._currentLookPoint.rotation;
        
        _blackboard._locomotion.Rotation(false);
    }

    protected override void OnStop()
    {
        _blackboard._locomotion.Rotation(true);
    }

    protected override State OnUpdate()
    {
        //if the current look point is null
        if(_blackboard._currentLookPoint == null)
        {
            //output error and return failure
            Debug.Log(_blackboard._agent.transform.name + ": [ERROR: TurnToLookPoint::OnUpdate]: Lookpoint is null");
            Debug.Break();
            return State.Failure;
        }

        //Get the current rotation
        Quaternion originalRot = _blackboard._agent.transform.rotation;
        //lerp between the original and the target rotation
        _blackboard._agent.transform.rotation = Quaternion.Lerp(originalRot, targetRot, Time.deltaTime * 5.0f);

        //get the original and new y values
        float oY = originalRot.eulerAngles.y;
        float nY = targetRot.eulerAngles.y;

        //if these are approximately the same
        if (Mathf.Approximately(oY, nY))
        {
            //then return successul
            return State.Success;
        }

        return State.Running;
    }
}