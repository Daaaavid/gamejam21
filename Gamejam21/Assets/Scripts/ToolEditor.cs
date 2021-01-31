using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DefaultNamespace.Tool))]
public class ToolEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        var t = (DefaultNamespace.Tool)target;

        if (GUILayout.Button("parent")) t.DoStuff();

        base.OnInspectorGUI();
    }
}