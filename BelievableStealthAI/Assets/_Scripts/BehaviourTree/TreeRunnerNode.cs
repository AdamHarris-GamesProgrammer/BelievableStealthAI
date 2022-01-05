using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeRunnerNode : ActionNode
{
    public BehaviorTree _treeToRun;

    protected override void OnStart()
    {
        _treeToRun = _treeToRun.Clone();
        _treeToRun._rootNode._state = State.Running;
        _treeToRun.Bind();
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        return _treeToRun.Update();
    }
}
