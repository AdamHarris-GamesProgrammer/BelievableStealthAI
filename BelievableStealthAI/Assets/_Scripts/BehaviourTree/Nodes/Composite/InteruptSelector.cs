using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteruptSelector : SelectorNode
{
    protected override State OnUpdate()
    {
        //Gets the current node
        int previous = _current;
        base.OnStart();

        //Gets the state from the Selector's update method
        var status = base.OnUpdate();

        //if the previously updating node is not the same as the current node then it means another node has executed successfully
        if (previous != _current)
        {
            //Abort the previous node
            if (_children[previous]._state == State.Running)
            {
                _children[previous].Abort();
            }
        }

        return status;
    }
}