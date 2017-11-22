using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// This class is used to show information for the ConnectionBrush in the inspector
    /// </summary>
    [CustomEditor(typeof(ConnectionBrush))]
    public class ConnectionBrushEditor : GridBrushEditor
    {
        //The current ConnectionBrush
        public ConnectionBrush Brush { get { return (target as ConnectionBrush); } }


        public override void OnPaintInspectorGUI()
        {
            base.OnPaintInspectorGUI();
            GUILayout.BeginHorizontal();
            //This shows a dropdown menu that tells the brush what type of tile it will place
            Brush.BrushTileType =
                (BrushTileType)EditorGUILayout.EnumPopup("TileFlag type", Brush.BrushTileType);
            GUILayout.EndHorizontal();
        }
    }
}


