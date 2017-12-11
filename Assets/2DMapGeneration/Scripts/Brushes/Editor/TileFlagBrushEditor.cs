using MapGeneration;
using UnityEditor;
using UnityEngine;

namespace MapGeneration.Editor
{
    /// <summary>
    /// This class is used to show information for the TileFlagBrush in the inspector
    /// </summary>
    [CustomEditor(typeof(TileFlagBrush))]
    public class TileFlagBrushEditor : GridBrushEditor
    {
        public TileFlagBrush Brush { get { return (target as TileFlagBrush); } }

        /// <summary>
        /// This handles the brush's inspector items.
        /// </summary>
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
}


