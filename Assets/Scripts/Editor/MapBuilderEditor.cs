using MapGeneration;
using UnityEditor;
using UnityEngine;

namespace MapBuilderEditor
{
    /// <summary>
    /// Purpose: Custom Inspector UI for Map Builder
    /// Creator: MP
    /// </summary>
    [CustomEditor(typeof(MapBuilder))]
    public class MapBuilderEditor : Editor
    {
        private MapBuilder _context;
        private SerializedProperty _savedMaps;
        private SerializedProperty _activeMap;

        void OnEnable()
        {
            _context = target as MapBuilder;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Generate"))
            {
                if (!Application.isPlaying && _context.ActiveMap != _context.PreExistingMap)
                    DestroyImmediate(_context.PreExistingMap.gameObject);

                _context.PreExistingMap = _context.Generate();
                EditorUtility.SetDirty(_context);
            }

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Pre-Existing Map", _context.PreExistingMap, typeof(Map), true);
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(10);

            _context.CurrentBlueprint = EditorGUILayout.ObjectField("Current Blueprint", _context.CurrentBlueprint, typeof(MapBlueprint), true) as MapBlueprint;
        }
    }
}