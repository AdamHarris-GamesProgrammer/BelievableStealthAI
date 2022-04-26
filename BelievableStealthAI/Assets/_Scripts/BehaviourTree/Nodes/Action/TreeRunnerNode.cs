using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeRunnerNode : ActionNode
{
    public BehaviorTree _treeToRun;

    protected override void OnStart()
    {
        //clones the behaviour tree selected
        _treeToRun = _treeToRun.Clone();

        //sets the blackboard for the new tree
        _treeToRun._blackboard = _blackboard;
        _treeToRun._rootNode._state = State.Running;
        //binds the tree
        _treeToRun.Bind();
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        //Update the subtree
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
