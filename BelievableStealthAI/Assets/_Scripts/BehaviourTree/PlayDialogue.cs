using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDialogue : ActionNode
{
    public SoundType _typeToPlay;


    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        _blackboard._agent.PlaySound(_typeToPlay);

        return State.Success;
    }
}