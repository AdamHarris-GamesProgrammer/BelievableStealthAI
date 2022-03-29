using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToLookPoint : ActionNode
{
    Quaternion targetRot;

    protected override void OnStart()
    {
        targetRot = _blackboard._currentLookPoint.rotation;
        
        _blackboard._locomotion.Rotation(false);
    }

    protected override void OnStop()
    {
        _blackboard._locomotion.Rotation(true);
    }

    protected override State OnUpdate()
    {
        if(_blackboard._currentLookPoint == null)
        {
            Debug.Log(_blackboard._agent.transform.name + ": [ERROR: TurnToLookPoint::OnUpdate]: Lookpoint is null");
            Debug.Break();
            return State.Failure;
        }

        Quaternion originalRot = _blackboard._agent.transform.rotation;
        _blackboard._agent.transform.rotation = Quaternion.Lerp(originalRot, targetRot, Time.deltaTime * 5.0f);


        float oY = originalRot.eulerAngles.y;
        float nY = targetRot.eulerAngles.y;

        if (Mathf.Approximately(oY, nY))
        {
            return State.Success;
        }

        return State.Running;
    }
}