using System.Collections;
using System.Collections.Generic;
using MapGeneration;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileFlagBrush))]
public class TileFlagBrushEditor : GridBrushEditor
{
    public TileFlagBrush Brush { get { return (target as TileFlagBrush); } }

    public override void OnPaintInspectorGUI()
    {
        base.OnPaintInspectorGUI();
        GUILayout.BeginHorizontal();
        //This shows a dropdown menu that tells the brush what type of tile it will place
        Brush.BrushTileFlag =
            (BrushTileFlag)EditorGUILayout.EnumPopup("TileFlag type", Brush.BrushTileFlag);
        GUILayout.EndHorizontal();
    }
}
