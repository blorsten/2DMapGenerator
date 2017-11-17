using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// This class is used to show information for the TileTypeBrush in the inspector
    /// </summary>
    [CustomEditor(typeof(TileTypeBrush))]
    public class TileTypeBrushEditor : GridBrushEditor
    {
        //The current TileTypeBrush
        public TileTypeBrush Brush { get { return (target as TileTypeBrush); } }


        public override void OnPaintInspectorGUI()
        {
            base.OnPaintInspectorGUI();
            GUILayout.BeginHorizontal();
            //This shows a dropdown menu that tells the brush what type of tile it will place
            Brush.BrushTileType =
                (TileType)EditorGUILayout.EnumPopup("Tile type", Brush.BrushTileType);
            GUILayout.EndHorizontal();
        }
    }
}


