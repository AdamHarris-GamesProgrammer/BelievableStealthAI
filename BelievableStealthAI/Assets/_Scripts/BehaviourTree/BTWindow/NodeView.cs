using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
#endif

#if UNITY_EDITOR
public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    public Node _node;

    //ports at the top and bottom of the node
    public Port _input;
    public Port _output;

    public Action<NodeView> OnNodeSelected;

    public NodeView(Node node) : base("Assets/_Scripts/BehaviourTree/BTWindow/NodeView.uxml") {
        _node = node;
        title = node.name;
        viewDataKey = node._guid;

        style.left = node._position.x;
        style.top = node._position.y;

        CreateInputPorts();
        CreateOutputPorts();
        SetupClasses();

        Label descriptionLabel = this.Q<Label>("description");
        descriptionLabel.bindingPath = "description";
        descriptionLabel.Bind(new SerializedObject(node));
    }

    public override void OnSelected()
    {
        base.OnSelected();
        if (OnNodeSelected != null)
            OnNodeSelected.Invoke(this);
    }

    public void SortChildren()
    {
        //if the node is a composite then sort the list by their position
        CompositeNode composite = _node as CompositeNode;
        if (composite) composite._children.Sort(SortByHorizontalPosition);
    }

    public void UpdateState()
    {
        //removes the class specifier in the editor, this stops them from being a different colour
        RemoveFromClassList("running");
        RemoveFromClassList("success");
        RemoveFromClassList("failure");

        //if we are playing
        if (Application.isPlaying)
        {
            //add a class specifier depending on the state the node is in
            switch (_node._state)
            {
                case Node.State.Running:
                    if (_node._started) AddToClassList("running");
                    break;
                case Node.State.Failure:
                    AddToClassList("failure");
                    break;
                case Node.State.Success:
                    AddToClassList("success");
                    break;
            }
        }

    }

    public override void SetPosition(Rect newPos)
    {
        //Sets the position of the node
        base.SetPosition(newPos);
        Undo.RecordObject(_node, "Behaviour Tree (Set Position)");
        
        _node._position.x = newPos.xMin;
        _node._position.y = newPos.yMin;
        EditorUtility.SetDirty(_node);
    }

    private void SetupClasses()
    {
        //This allows for the top bar to be the desired colour
        if (_node is ActionNode) AddToClassList("action");
        else if (_node is CompositeNode) AddToClassList("composite");
        else if (_node is DecoratorNode) AddToClassList("decorator");
        else if (_node is RootNode) AddToClassList("root");
    }

    private void CreateInputPorts()
    {
        //Adds input ports to the node

        if(_node is ActionNode)
            _input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        else if(_node is CompositeNode)
            _input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        else if(_node is DecoratorNode)
            _input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));

        if(_input != null)
        {
            _input.portName = "";
            _input.style.flexDirection = FlexDirection.Column;
            inputContainer.Add(_input);
        }
    }

    private void CreateOutputPorts()
    {
        //Creates a output port based on the type of node this is. Composite nodes have multiple children so they have a multi port.
        if (_node is ActionNode) {}
        else if (_node is CompositeNode)
            _output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
        else if (_node is DecoratorNode)
            _output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
        else if (_node is RootNode)
            _output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));

        if (_output != null)
        {
            _output.portName = "";
            _output.style.flexDirection = FlexDirection.ColumnReverse;
            outputContainer.Add(_output);
        }
    }

    private int SortByHorizontalPosition(Node left, Node right) => left._position.x < right._position.x ? -1 : 1;
}
#endif
