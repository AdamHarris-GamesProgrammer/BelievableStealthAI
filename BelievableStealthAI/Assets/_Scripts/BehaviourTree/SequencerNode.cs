using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerNode : CompositeNode
{
    int _current = 0;

    protected override void OnStart()
    {
        _current = 0;
        Debug.Log("Sequence on start");
    }
    protected override void OnStop() {}

    protected override State OnUpdate()
    {
        var currentChild = _children[_current];

        switch (currentChild.Update())
        {
            case State.Running:
                return State.Running;
            case State.Failure:
                return State.Failure;
            case State.Success:
                _current++;
                break;
        }

        //if current is equal to the amount of children then return success if not return running
        return _current == _children.Count ? State.Success : State.Running;
    }
}