using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeRunnerNode : ActionNode
{
    public BehaviorTree _treeToRun;

    protected override void OnStart()
    {
        _treeToRun = _treeToRun.Clone();
        _treeToRun._blackboard = _blackboard;
        _treeToRun._rootNode._state = State.Running;
        _treeToRun.Bind();
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        State state = _treeToRun.Update();
        if(state == State.Failure || state == State.Success)
        {
            //Update the original blackboard
            _blackboard = _treeToRun._blackboard;
            return state;
        }
        return State.Running;
    }
}
