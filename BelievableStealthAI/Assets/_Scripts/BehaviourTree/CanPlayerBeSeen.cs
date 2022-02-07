using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanPlayerBeSeen : DecoratorNode
{
    protected override void OnStart()
    {
        //Debug.Log("CanPlayerBeSeen Start");
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (_blackboard._player.GetComponent<Health>().IsDead) return State.Failure;

        //Debug.Log("Seeing player: " + _blackboard._agent.CurrentlySeeingPlayer);
        if(_blackboard._agent.CurrentlySeeingPlayer)
        {
            return child.Update();
        }
        else
        {
            return State.Failure;
        }
    }
}
