using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfPOI : DecoratorNode
{
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        State state = child.Update();
        if(state == State.Failure || state == State.Success)
        {
            if(_blackboard._agent.CurrentRoom == null)
            {
                Debug.Log("[ERROR: OutOfPOI::OnUpdate]: Current room is null");
                return State.Success;
            }
            if (_blackboard._agent.CurrentRoom.OutOfPOIs())
            {
                return State.Success;
            }
        }

        return State.Running;
    }
}