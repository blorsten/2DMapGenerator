using MapGeneration.ChunkSystem;
using UnityEditor;
using UnityEngine;

namespace MapGeneration.Editor
{
    /// <summary>
    /// A class for the chunk custom inspector.
    /// </summary>
    [CustomEditor(typeof(Chunk))]
    public class ChunkEditor : UnityEditor.Editor
    {
        private Chunk _chunk;

        private void OnEnable()
        {
            _chunk = target as Chunk;
        }

        /// <summary>
        /// This draws the chunk's custom inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            if (_chunk.RecipeReference)
                if (GUILayout.Button("Find Recipe"))
                    EditorGUIUtility.PingObject(_chunk.RecipeReference.gameObject);

            base.OnInspectorGUI();
        }
    }

}