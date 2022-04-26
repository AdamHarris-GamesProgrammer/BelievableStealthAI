using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
using UnityEngine.UIElements;
using UnityEditor.Callbacks;
#endif
using System;

#if UNITY_EDITOR
public class BehaviorTreeEditor : EditorWindow
{
    BehaviorTreeView _treeView;
    InspectorView _inspectorView;
    IMGUIContainer _blackboardView;

    SerializedObject _treeObject;
    SerializedProperty _blackboardProperty;

    [MenuItem("AI/BehaviorTreeEditor")]
    public static void OpenWindow()
    {
        BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviorTreeEditor");
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        if (Selection.activeObject is BehaviorTree)
        {
            OpenWindow();
            return true;
        }

        return false;
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/_Scripts/BehaviourTree/BTWindow/BehaviorTreeEditor.uxml");
        visualTree.CloneTree(root);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/_Scripts/BehaviourTree/BTWindow/BehaviorTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        _treeView = root.Q<BehaviorTreeView>();
        _treeView.FrameOrigin();
        _inspectorView = root.Q<InspectorView>();
        _blackboardView = root.Q<IMGUIContainer>();
        _blackboardView.onGUIHandler = () =>
        {
            _treeObject?.Update();
            if(_blackboardProperty != null)
                EditorGUILayout.PropertyField(_blackboardProperty);
            _treeObject?.ApplyModifiedProperties();
        };

        _treeView.OnNodeSelected += OnNodeSelectionChanged;

        OnSelectionChange();
    }

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private void OnPlayModeStateChanged(PlayModeStateChange obj)
    {
        switch (obj)
        {
            case PlayModeStateChange.EnteredEditMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingEditMode:
                break;
            case PlayModeStateChange.EnteredPlayMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                break;
        }
    }
    private void OnDisable() => EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

    private void OnSelectionChange()
    {
        BehaviorTree tree = Selection.activeObject as BehaviorTree;

        //if we do not have a tree 
        if (!tree)
        {
            //if we have a selected game object
            if (Selection.activeGameObject)
            {
                //if that object has a behaviour tree
                BehaviorTreeRunner runner = Selection.activeGameObject.GetComponent<BehaviorTreeRunner>();

                //Display the tree of that agent.
                if (runner)
                {
                    tree = runner.tree;
                }
            }
        }

        if(_treeView == null)
        {
            VisualElement root = rootVisualElement;
            _treeView = root.Q<BehaviorTreeView>();
        }

        if (Application.isPlaying)
        {
            //if we have a tree
            if (tree)
            {
                //populate the tree view with the content of the tree
                _treeView?.PopulateView(tree);
            }
        }
        else
        {
            //if we have a tree and can open the editor
            if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
            {
                //populate the view with the contents of the tree.
                _treeView?.PopulateView(tree);
            }
        }

        //if we have a tree
        if(tree != null)
        {
            //Serialize the object allowing us to modify it and display the blackboard in the editor
            _treeObject = new SerializedObject(tree);
            _blackboardProperty = _treeObject.FindProperty("_blackboard");
        }
    }

    private void OnNodeSelectionChanged(NodeView node) => _inspectorView.UpdateSelection(node);

    private void OnInspectorUpdate() => _treeView?.UpdateNodeState();
}
#endif
