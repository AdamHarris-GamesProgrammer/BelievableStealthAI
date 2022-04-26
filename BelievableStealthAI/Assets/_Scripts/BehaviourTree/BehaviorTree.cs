using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName ="AI/Behavior Tree/New Behavior Tree")]
public class BehaviorTree : ScriptableObject
{
    public Node _rootNode;

    public Node.State _treeState = Node.State.Running;
    public List<Node> _nodes = new List<Node>();

    public Blackboard _blackboard = new Blackboard();

    public string currentNodeName;

    public Node.State Update()
    {
        if(_rootNode._state == Node.State.Running) _treeState = _rootNode.Update();
        return _treeState;
    }

#if UNITY_EDITOR
    public Node CreateNode(System.Type type)
    {
        Node node = ScriptableObject.CreateInstance(type) as Node;
        node.name = type.Name;
        node._guid = GUID.Generate().ToString();
        Undo.RecordObject(this, "Behaviour Tree (Create Node)");
        _nodes.Add(node);

        if (!Application.isPlaying) AssetDatabase.AddObjectToAsset(node, this);

        Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree (Create Node");
        AssetDatabase.SaveAssets();
        return node;
    }
    public void DeleteNode(Node node)
    {
        Undo.RecordObject(this, "Behaviour Tree (Create Node)");
        _nodes.Remove(node);
        //AssetDatabase.RemoveObjectFromAsset(node);
        Undo.DestroyObjectImmediate(node);
        AssetDatabase.SaveAssets();
    }

    public void AddChild(Node parent, Node child)
    {
        RootNode rootNode = parent as RootNode;
        if (rootNode)
        {
            Undo.RecordObject(rootNode, "Behaviour Tree (Add Child)");
            rootNode.child = child;
            EditorUtility.SetDirty(rootNode);
        }

        DecoratorNode decorator = parent as DecoratorNode;
        if(decorator)
        {
            Undo.RecordObject(decorator, "Behaviour Tree (Add Child)");
            decorator.child = child;
            EditorUtility.SetDirty(decorator);
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            Undo.RecordObject(composite, "Behaviour Tree (Add Child)");
            composite._children.Add(child);
            EditorUtility.SetDirty(composite);
        }
    }

    public void RemoveChild(Node parent, Node child)
    {
        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator)
        {
            Undo.RecordObject(decorator, "Behaviour Tree (Add Child)");
            decorator.child = null;
            EditorUtility.SetDirty(decorator);
        }

        RootNode rootNode = parent as RootNode;
        if (rootNode)
        {
            Undo.RecordObject(rootNode, "Behaviour Tree (Add Child)");
            rootNode.child = null;
            EditorUtility.SetDirty(rootNode);
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            Undo.RecordObject(composite, "Behaviour Tree (Add Child)");
            composite._children.Remove(child);
            EditorUtility.SetDirty(composite);
        }
    }
#endif

    public static List<Node> GetChildren(Node parent)
    {
        //if the parent is a decorator node then add the child
        List<Node> children = new List<Node>();
        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator && decorator.child != null) children.Add(decorator.child);

        //if the parent is a root node then add the child
        RootNode rootNode = parent as RootNode;
        if (rootNode && rootNode.child != null) children.Add(rootNode.child);

        //If the parent is a composite node then add all the children
        CompositeNode composite = parent as CompositeNode;
        if (composite) return composite._children;

        return children;
    }

    public static void Traverse(Node node, System.Action<Node> visiter)
    {
        if(node)
        {
            visiter.Invoke(node);
            var children = GetChildren(node);
            children.ForEach((n) => Traverse(n, visiter));
        }
    }


    public BehaviorTree Clone()
    {
        //Creates a new object of this tree
        BehaviorTree tree = Instantiate(this);

        //Clones all nodes into the new tree
        tree._rootNode = tree._rootNode.Clone();
        tree._nodes = new List<Node>();

        //Traverses the tree
        Traverse(tree._rootNode, (n) => { tree._nodes.Add(n); });

        return tree;
    }

    public void Bind() => Traverse(_rootNode, node => { node._blackboard = _blackboard; node._agent = _blackboard._agent; });
}