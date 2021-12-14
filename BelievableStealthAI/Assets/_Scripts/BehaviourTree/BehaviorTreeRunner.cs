using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeRunner : MonoBehaviour
{
    BehaviorTree tree;

    // Start is called before the first frame update
    void Start()
    {
        tree = ScriptableObject.CreateInstance<BehaviorTree>();

        var a = ScriptableObject.CreateInstance<DebugLogNode>();
        a.message = "Hello";

        var b = ScriptableObject.CreateInstance<DebugLogNode>();
        b.message = "Potato";

        var c = ScriptableObject.CreateInstance<DebugLogNode>();
        c.message = "Goodbye";

        var sequencerNode = ScriptableObject.CreateInstance<SequencerNode>();
        var waitNode = ScriptableObject.CreateInstance<WaitNode>();
        var repeatNode = ScriptableObject.CreateInstance<RepeatNode>();
        repeatNode.iterations = 5;
        repeatNode.child = b;

        sequencerNode.children.Add(a);
        sequencerNode.children.Add(waitNode);
        sequencerNode.children.Add(repeatNode);
        sequencerNode.children.Add(waitNode);
        sequencerNode.children.Add(c);


        tree.rootNode = sequencerNode;
    }

    // Update is called once per frame
    void Update()
    {
        tree.Update();
    }
}
