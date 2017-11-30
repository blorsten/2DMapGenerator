using System;
using MapGeneration;
using UnityEditor;
using UnityEngine;

namespace MapGeneration.Editor
{
    /// <summary>
    /// Purpose: Custom inspector UI for Map component.
    /// Creator: MP
    /// </summary>
    [CustomEditor(typeof(Map))]
    public class MapEditor : UnityEditor.Editor
    {
        private Map _context;

        void OnEnable()
        {
            _context = target as Map;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Info:", EditorStyles.boldLabel);

            if (_context.ID != Guid.Empty && Application.isPlaying)
                EditorGUILayout.LabelField(string.Format("ID: {0}", _context.ID));

            EditorGUILayout.LabelField(string.Format("Seed: {0}", _context.Seed));

            EditorGUI.BeginDisabledGroup(true);
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_mapBlueprint"));

                GUILayout.Space(10);
                if (Application.isPlaying)
                {
                    EditorGUILayout.LabelField("Chunk Info:", EditorStyles.boldLabel);

                    if (_context.StartChunk != null)
                        EditorGUILayout.ObjectField("Start Chunk", _context.StartChunk.Instance, typeof(Chunk), false);

                    if (_context.EndChunk != null)
                        EditorGUILayout.ObjectField("End Chunk", _context.EndChunk.Instance, typeof(Chunk), false);
                }
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}