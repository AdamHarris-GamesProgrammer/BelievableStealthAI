using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : ActionNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(_blackboard._player.GetComponent<Health>().IsDead)
        {
            return State.Success;
        }

        if(!_blackboard._agent.TryAttack())
        {
            return State.Failure;            
        }


        _blackboard._agent.Attack();

        return State.Success;
    }
}