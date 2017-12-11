using System;
using System.Collections.Generic;
using System.Linq;
using MapGeneration;
using MapGeneration.Editor;
using MapGeneration.Extensions;
using MapGeneration.SaveSystem;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MapGeneration.Editor
{
    /// <summary>
    /// Purpose: Custom Inspector UI for Map Builder
    /// Creator: MP
    /// </summary>
    [CustomEditor(typeof(MapBuilder))]
    [Serializable]
    public class MapBuilderEditor : UnityEditor.Editor
    {
        private const int MAX_SAVED_SEEDS = Int32.MaxValue;
        private const int SCROLLVIEW_LIMIT = 5;
        private const int SCROLLVIEW_ENTRY_HEIGHT = 20;

        private Vector2 _scrollPos;
        private MapBuilder _context; //Used to get hold of the serialized object in the inspector.

        private MapBlueprintEditor.ReorderableAlgorithmStack _reorderableAlgorithmStack;
        private SerializedObject _serializedBlueprint;

        void OnEnable()
        {
            _context = target as MapBuilder; //Safely cast the targeted serialized object as a map builder.
        }

        public override void OnInspectorGUI()
        { 
            if (!_context.gameObject.activeInHierarchy)
                return;

            //If the user presses generate, generate a map.
            if (GUILayout.Button("Generate"))
                Generate();

            GUILayout.Space(10);

            #region SEED HISTORY
            if (_context.SavedSeeds != null && _context.SavedSeeds.Any())
            {
                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Seed History:", EditorStyles.boldLabel);
                    if (GUILayout.Button("CLEAR"))
                    {
                        _context.SavedMaps.Clear();
                        _context.SavedSeeds.Clear();
                        EditorUtility.SetDirty(_context);
                    }
                }
                GUILayout.EndHorizontal();

                if (_context.SavedSeeds.Count > SCROLLVIEW_LIMIT)
                    _scrollPos = EditorGUILayout.BeginScrollView(
                        _scrollPos, 
                        false, 
                        true, 
                        GUIStyle.none,
                        GUI.skin.verticalScrollbar, GUI.skin.scrollView,
                        GUILayout.Height((SCROLLVIEW_LIMIT * SCROLLVIEW_ENTRY_HEIGHT) + 10));

                for (var index = _context.SavedSeeds.Count - 1; index >= 0; index--)
                {
                    int savedSeed = _context.SavedSeeds[index];
                    GUILayout.BeginHorizontal(GUILayout.Height(SCROLLVIEW_ENTRY_HEIGHT));
                    {
                        GUILayout.Label(savedSeed.ToString(), new GUIStyle(GUI.skin.label)
                        {
                            fontStyle = _context.PreExistingMap && savedSeed == _context.PreExistingMap.Seed ? FontStyle.BoldAndItalic : FontStyle.Normal
                        });

                        //Try and see if theres any saved data.
                        MapDataSaver foundData = _context.SavedMaps.FirstOrDefault(saver => saver.MapSeed == savedSeed);
                        if (foundData != null && foundData.HasSavedData)
                            if (GUILayout.Button("CLEAR DATA", GUILayout.Width(100)))
                                _context.SavedMaps.Remove(foundData);

                        if (GUILayout.Button("LOAD", GUILayout.Width(50)))
                            Generate(savedSeed);
                    }
                    GUILayout.EndHorizontal();
                }

                if (_context.SavedSeeds.Count > SCROLLVIEW_LIMIT)
                    EditorGUILayout.EndScrollView();
            }

            GUILayout.Space(10);
            #endregion

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

            if (_context.CurrentBlueprint)
            {
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Current Algorithm Stack:", EditorStyles.boldLabel);
                if (_reorderableAlgorithmStack == null || _context.CurrentBlueprint != _serializedBlueprint.targetObject)
                {
                    if (Event.current.type == EventType.Repaint)
                    {
                        _serializedBlueprint = new SerializedObject(_context.CurrentBlueprint);
                        _reorderableAlgorithmStack =
                            new MapBlueprintEditor.ReorderableAlgorithmStack(_serializedBlueprint,
                                _serializedBlueprint.FindProperty("AlgorithmStack"),
                                _context.CurrentBlueprint.AlgorithmStack, true);
                    }
                }
                else
                {
                    _reorderableAlgorithmStack.List.DoLayoutList();
                    GUILayout.Space(10);
                    _serializedBlueprint.ApplyModifiedProperties();
                }
            }
        }

        private void Generate(int seed = 0)
        {
            //If we went from playing to editor and generates a new map, destroy the old one.
            if (!Application.isPlaying && _context.PreExistingMap &&
                _context.PreExistingMap.gameObject &&
                _context.ActiveMap != _context.PreExistingMap)
                DestroyImmediate(_context.PreExistingMap.gameObject);

            //Try to find a MapDataSaver with that seed.
            MapDataSaver foundDataSaver = _context.SavedMaps.FirstOrDefault(saver => saver.MapSeed == seed);

            //Set the preexisting map to the one we just generated, or take an existing saved map.
            _context.PreExistingMap = foundDataSaver != null ? _context.Generate(foundDataSaver) : _context.Generate(seed);

            if (!_context.PreExistingMap)
                return;

            //If the seeds is not saved yet, do so.
            if (!_context.SavedSeeds.Contains(_context.PreExistingMap.Seed))
            {
                //If wee exceed max amount of saved seeds remove the first one.
                if (_context.SavedSeeds.Count >= MAX_SAVED_SEEDS)
                    _context.SavedSeeds.RemoveAt(0);

                _context.SavedSeeds.Add(_context.PreExistingMap.Seed);
            }

            //After we set edited to serialized object, mark it as dirty.
            EditorUtility.SetDirty(_context); 
            if(!Application.isPlaying)
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }
}