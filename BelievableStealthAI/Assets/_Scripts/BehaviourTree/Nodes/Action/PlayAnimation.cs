using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : ActionNode
{
    public string animationName = "";

    protected override void OnStart()
    {
        //Sets the in animation value
        _blackboard._agent.IsInAnimation = true;
        //play the animation with the desired name
        _blackboard._agent.Anim.Play(animationName, -1, 0.0f);
    }

    protected override void OnStop()
    {
        //if the agent is still in the animation
        if (_blackboard._agent.IsInAnimation)
        {
            //Force stop the animation
            _blackboard._agent.ForceStopAnimtion();
        }
    }

    protected override State OnUpdate()
    {
        //if the agent is no longer in the animation then return successful
        if (!_blackboard._agent.IsInAnimation)
        {
            return State.Success;
        }

        //return running
        return State.Running;
    }
}