using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.GraphView;
#endif
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
public class BehaviorTreeView : GraphView
{
    public new class UxmlFactory : UxmlFactory<BehaviorTreeView, GraphView.UxmlTraits> { }

    public Action<NodeView> OnNodeSelected;
    BehaviorTree _tree;

    public BehaviorTreeView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new SelectionDropper());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/_Scripts/BehaviourTree/BTWindow/BehaviorTreeEditor.uss");
        styleSheets.Add(styleSheet);

        Undo.undoRedoPerformed += OnUndoRedo;
    }

    public void UpdateNodeState()
    {
        nodes.ForEach(n =>
        {
            NodeView view = n as NodeView;
            view.UpdateState();
        });
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        VisualElement contentViewContainer = ElementAt(1);
        Vector3 screenMousePosition = evt.localMousePosition;
        Vector2 worldMousePosition = screenMousePosition - contentViewContainer.transform.position;
        worldMousePosition *= 1 / contentViewContainer.transform.scale.x;

        //Gets all of the types derived from the node class.
        var nodeTypes = TypeCache.GetTypesDerivedFrom<Node>();
        foreach (var type in nodeTypes)
        {
            if (type.BaseType.Name == "Node") continue;
            evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type, worldMousePosition));
        }
        
    }

    public void PopulateView(BehaviorTree tree)
    {
        _tree = tree;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        if (tree._rootNode == null)
        {
            tree._rootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
            EditorUtility.SetDirty(tree);
            AssetDatabase.SaveAssets();
        }

        //Create each of the children in the editor
        tree._nodes.ForEach(n => CreateNodeView(n));

        //Create the edges between the node and their children
        tree._nodes.ForEach(n =>
        {
            var children = BehaviorTree.GetChildren(n);
            children.ForEach(child =>
            {
                NodeView parentView = FindNodeView(n);
                NodeView childView = FindNodeView(child);

                Edge edge = parentView._output.ConnectTo(childView._input);
                AddElement(edge);
            });
        });
    }

    private void OnUndoRedo()
    {
        PopulateView(_tree);
        AssetDatabase.SaveAssets();
    }

    private NodeView FindNodeView(Node node)
    {
        return GetNodeByGuid(node._guid) as NodeView;
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if(graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                //casts the element to a node view and deletes it if needed
                NodeView nodeView = elem as NodeView;
                if (nodeView != null)
                {
                    _tree.DeleteNode(nodeView._node);
                }

                //Casts the element to an edge
                Edge edge = elem as Edge;
                if (edge != null)
                {
                    //Removes the edge 
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    _tree.RemoveChild(parentView._node, childView._node);
                }
            });
        }

        //if we have edges to create
        if(graphViewChange.edgesToCreate != null)
        {
            //Cycle through each edge
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                _tree.AddChild(parentView._node, childView._node);
            });
        }

        //Sorts the children of a composite node if needed
        if(graphViewChange.movedElements != null)
        nodes.ForEach((n) => { NodeView view = n as NodeView; view.SortChildren(); });
        return graphViewChange;
    }

    private void CreateNode(System.Type type, Vector2 mousePos)
    {
        if(_tree != null)
        {
            Node node = _tree.CreateNode(type);
            CreateNodeView(node, mousePos);
        }
    }

    //Creates a node view at the mouse pos
    private void CreateNodeView(Node node, Vector2 mousePos)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        Rect nodePos = new Rect(mousePos, mousePos);
        nodeView.SetPosition(nodePos);
        AddElement(nodeView);
    }

    private void CreateNodeView(Node node)
    {
        //Creates a new node view and adds it as an element
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
    }
}
#endif