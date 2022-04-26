using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToPOI : ActionNode
{
    //Stores a target rotation
    Quaternion targetRot;

    protected override void OnStart()
    {
        //Get the rotation of the investigation point
        targetRot = _blackboard._currentPOI.InvestigationPoint.rotation;

        //disable agent rotation
        _blackboard._locomotion.Rotation(false);
    }

    protected override void OnStop()
    {
        //enable agent rotation
        _blackboard._locomotion.Rotation(true);   
    }

    protected override State OnUpdate()
    {
        //if we have no current POI
        if (_blackboard._currentPOI == null)
        {
            //log error and return failure
            Debug.Log(_blackboard._agent.transform.name + ": [ERROR: TurnToLookPoint::OnUpdate]: Lookpoint is null");
            Debug.Break();
            return State.Failure;
        }

        //Gets the current rotation
        Quaternion originalRot = _blackboard._agent.transform.rotation;
        //Lerps between the original rot and the target rot
        _blackboard._agent.transform.rotation = Quaternion.Lerp(originalRot, targetRot, Time.deltaTime * 5.0f);

        //Gets the y axis value of the original and new value
        float oY = originalRot.eulerAngles.y;
        float nY = targetRot.eulerAngles.y;

        //if the values are approximately the same
        if (Mathf.Approximately(oY, nY))
        {
            //rotation successful
            return State.Success;
        }

        //Running
        return State.Running;
    }
}