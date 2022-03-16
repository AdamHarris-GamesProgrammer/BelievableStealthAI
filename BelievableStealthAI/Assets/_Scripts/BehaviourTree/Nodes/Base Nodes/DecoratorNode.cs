using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecoratorNode : Node
{
    public Node child;

    public override Node Clone()
    {
        DecoratorNode node = Instantiate(this);
        if(node.child != null)
            node.child = child.Clone();
        return node;
    }
}
