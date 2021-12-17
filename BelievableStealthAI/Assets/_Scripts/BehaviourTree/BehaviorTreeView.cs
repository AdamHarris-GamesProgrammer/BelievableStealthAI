using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

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

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/_Scripts/BehaviourTree/BehaviorTreeEditor.uss");
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
        var nodeTypes = TypeCache.GetTypesDerivedFrom<Node>();
        foreach (var type in nodeTypes)
        {
            if (type.BaseType.Name == "Node") continue;
            evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
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

        tree._nodes.ForEach(n => CreateNodeView(n));

        tree._nodes.ForEach(n =>
        {
            var children = tree.GetChildren(n);
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
                NodeView nodeView = elem as NodeView;
                if (nodeView != null)
                {
                    _tree.DeleteNode(nodeView._node);
                }

                Edge edge = elem as Edge;
                if (edge != null)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    _tree.RemoveChild(parentView._node, childView._node);
                }
            });
        }

        if(graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                _tree.AddChild(parentView._node, childView._node);
            });
        }

        if(graphViewChange.movedElements != null)
        nodes.ForEach((n) => { NodeView view = n as NodeView; view.SortChildren(); });
        return graphViewChange;
    }

    private void CreateNode(System.Type type)
    {
        if(_tree != null)
        {
            Node node = _tree.CreateNode(type);
            CreateNodeView(node);
        }
    }

    private void CreateNodeView(Node node)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
    }
}
