using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : ActionNode
{
    public string animationName = "";

    protected override void OnStart()
    {
        _blackboard._agent.IsInAnimation = true;
        _blackboard._agent.Anim.Play(animationName);
    }

    protected override void OnStop()
    {
        if (_blackboard._agent.IsInAnimation)
        {
            _blackboard._agent.ForceStopAnimtion();
        }
    }

    protected override State OnUpdate()
    {
        if (!_blackboard._agent.IsInAnimation)
        {
            return State.Success;
        }

        return State.Running;
    }
}