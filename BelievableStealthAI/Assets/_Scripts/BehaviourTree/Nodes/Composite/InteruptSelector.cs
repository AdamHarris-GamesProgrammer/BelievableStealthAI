using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteruptSelector : SelectorNode
{
    protected override State OnUpdate()
    {
        int previous = _current;
        base.OnStart();
        var status = base.OnUpdate();

        if (previous != _current)
        {
            if (_children[previous]._state == State.Running)
            {
                _children[previous].Abort();
            }
        }

        return status;
    }
}