using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEngine.UIElements;
using UnityEditor;
#endif

#if UNITY_EDITOR
public class InspectorView : VisualElement
{

    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }


    Editor editor;
    public InspectorView()
    {

    }

    internal void UpdateSelection(NodeView nodeView)
    {
        Clear();

        UnityEngine.Object.DestroyImmediate(editor);

        editor = Editor.CreateEditor(nodeView._node);
        IMGUIContainer container = new IMGUIContainer(() => { if(editor.target) editor.OnInspectorGUI(); });
        Add(container);

    }
}
#endif
