using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MapGeneration
{

    [CustomEditor(typeof(ConnectionBrush))]
    public class ConnectionBrushEditor : GridBrushEditorBase
    {
        public ConnectionBrush Brush { get { return (target as ConnectionBrush); } }


        public override void OnPaintInspectorGUI()
        {
            base.OnPaintInspectorGUI();
            GUILayout.BeginHorizontal();
            Brush.BrushConnectionType =
                (ConnectionType)EditorGUILayout.EnumPopup("Connection type", Brush.BrushConnectionType);
            GUILayout.EndHorizontal();

        }
    }
}


