using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : CompositeNode
{
    protected int _current = 0;

    protected override void OnStart()
    {
        _current = 0;
    }

    protected override void OnStop() {}

    protected override State OnUpdate()
    {
        //Loop through each child node
        for (int i = _current; i < _children.Count; ++i)
        {
            //Set current to i
            _current = i;
            var child = _children[_current];

            //Execute the current child
            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Success:
                    return State.Success;
                case State.Failure:
                    continue;
            }
        }

        return State.Failure;
    }
}
