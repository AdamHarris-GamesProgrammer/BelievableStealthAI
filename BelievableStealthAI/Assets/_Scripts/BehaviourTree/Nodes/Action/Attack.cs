using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : ActionNode
{
    protected override void OnStart()
    {
        _blackboard._locomotion.CanMove(false);
    }

    protected override void OnStop()
    {
        _blackboard._locomotion.CanMove(true);
    }

    protected override State OnUpdate()
    {
        if(_blackboard._player.GetComponent<Health>().IsDead)
        {
            return State.Success;
        }

        //Debug.Log("Attacking");

        _blackboard._agent.Attack();

        return State.Success;
    }
}