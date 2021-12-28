using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : CompositeNode
{
    int _current = 0;

    protected override void OnStart()
    {
        _current = 0;
    }

    protected override void OnStop() {}

    protected override State OnUpdate()
    {
        var currentChild = _children[_current];

        switch (currentChild.Update())
        {
            case State.Running:
                return State.Running;
                break;
            case State.Failure:
                _current++;
                break;
            case State.Success:
                return State.Success;
                break;
        }

        //if current is equal to the amount of children then return success if not return running
        return _current == _children.Count ? State.Failure : State.Running;
    }
}
