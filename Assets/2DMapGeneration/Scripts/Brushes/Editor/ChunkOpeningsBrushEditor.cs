using UnityEditor;
using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// This class is used to show information for the ChunkOpeningsBrush in the inspector
    /// </summary>
    [CustomEditor(typeof(ChunkOpeningsBrush))]
    public class ChunkOpeningsBrushEditor : GridBrushEditor
    {
        //The current ChunkOpeningsBrush
        public ChunkOpeningsBrush Brush { get { return (target as ChunkOpeningsBrush); } }

        /// <summary>
        /// This handles the brush's inspector items.
        /// </summary>
        public override void OnPaintInspectorGUI()
        {
            base.OnPaintInspectorGUI();
            GUILayout.BeginHorizontal();
            //This shows a dropdown menu that tells the brush what type of tile it will place
            Brush.ConnectionType =
                (ConnectionType)EditorGUILayout.EnumPopup("TileFlag type", Brush.ConnectionType);
            GUILayout.EndHorizontal();
        }
    }
}


