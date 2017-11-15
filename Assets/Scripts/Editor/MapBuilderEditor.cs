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
        private MapBuilder _context; //Used to get hold of the serialized object in the inspector.

        void OnEnable()
        {
            _context = target as MapBuilder; //Safely cast the targeted serialized object as a map builder.
        }

        public override void OnInspectorGUI()
        {
            //If the user presses generate, generate a map.
            if (GUILayout.Button("Generate"))
            {
                //If we went from playing to editor and generates a new map, destroy the old one.
                if (!Application.isPlaying && _context.PreExistingMap && 
                    _context.PreExistingMap.gameObject &&
                    _context.ActiveMap != _context.PreExistingMap )
                    DestroyImmediate(_context.PreExistingMap.gameObject);

                //Set the preexisting map to the one we just generated.
                _context.PreExistingMap = _context.Generate();

                //After we set edited to serialized object, mark it as dirty.
                EditorUtility.SetDirty(_context);
            }

            //If we're not in play mode show what has been chosen as the preexisting map.
            if (!Application.isPlaying)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField("Pre-Existing Map", _context.PreExistingMap, typeof(Map), true);
                EditorGUI.EndDisabledGroup();
            }

            GUILayout.Space(10);

            //Also make it posible to change the blueprint.
            _context.CurrentBlueprint = EditorGUILayout.ObjectField("Current Blueprint", _context.CurrentBlueprint, typeof(MapBlueprint), true) as MapBlueprint;
        }
    }
}