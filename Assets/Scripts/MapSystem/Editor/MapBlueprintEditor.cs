using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MapGeneration.Algorithm;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Zios;
using Zios.Interface;

namespace MapGeneration.Editor
{
    /// <summary>
    /// Purpose:
    /// Creator:
    /// </summary>
    [CustomEditor(typeof(MapBlueprint))]
    public class MapBlueprintEditor : UnityEditor.Editor
    {
        private const float ENTRY_OFFSET = 5;

        private readonly Dictionary<int, float> _heights = new Dictionary<int, float>();
        private MapBlueprint _context;
        private SerializedProperty _whitelistedChunks;
        private SerializedProperty _blacklistedChunks;
        private ReorderableList _algorithmStack;

        private void OnEnable()
        {
            _context = target as MapBlueprint;
            _whitelistedChunks = serializedObject.FindProperty("WhitelistedChunks");
            _blacklistedChunks = serializedObject.FindProperty("BlacklistedChunks");

            _algorithmStack = new ReorderableList(serializedObject,
                serializedObject.FindProperty("AlgorithmStack"),
                true, false, true, true);

            _algorithmStack.onAddDropdownCallback = (Rect buttonRect, ReorderableList l) =>
            {
                var menu = new GenericMenu();
                var algorithms = FindAssetsByType<MapGenerationAlgorithm>();
                foreach (var algorithm in algorithms)
                {
                    menu.AddItem(new GUIContent(algorithm.name), false, AlgorithmStackItemAdded, algorithm);
                }
                menu.ShowAsContext();
            };

            _algorithmStack.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    rect.y += 2;
                    var targetObject = _algorithmStack.serializedProperty.GetArrayElementAtIndex(index);

                    EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                        String.Empty,
                        targetObject.objectReferenceValue,
                        typeof(MapGenerationAlgorithm), false);

                    if (index == _algorithmStack.index)
                    {
                        var e = CreateEditor(targetObject.objectReferenceValue);

                        float yPos = rect.y + EditorGUIUtility.singleLineHeight + ENTRY_OFFSET;
                        float height = EditorGUIUtility.singleLineHeight + ENTRY_OFFSET;

                        if (e != null)
                        {
                            var so = e.serializedObject;
                            so.Update();

                            var prop = so.GetIterator();
                            prop.NextVisible(true);
                            while (prop.NextVisible(true))
                            {
                                EditorGUI.PropertyField(new Rect(rect.x, yPos, rect.width, EditorGUIUtility.singleLineHeight), prop);
                                yPos += EditorGUIUtility.singleLineHeight + ENTRY_OFFSET;
                                height += EditorGUIUtility.singleLineHeight + ENTRY_OFFSET;
                            }
                            if (GUI.changed)
                                so.ApplyModifiedProperties();
                        }

                        if (_heights.ContainsKey(index))
                            _heights[index] = height;
                        else
                            _heights.Add(index, height);
                    }
                    else
                    {
                        if (_heights.ContainsKey(index))
                            _heights.Remove(index);
                    }
                };

            _algorithmStack.elementHeightCallback = (index) =>
            {
                Repaint();

                if (_heights.ContainsKey(index))
                    return _heights[index];

                return 20;
            };
        }

        private void AlgorithmStackItemAdded(object userData)
        {
            _context.AlgorithmStack.Add((MapGenerationAlgorithm) userData);
            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            //Auto settings
            EditorGUILayout.LabelField("Automation Settings:", EditorStyles.boldLabel);
            _context.FindValidChunks = EditorGUILayout.Toggle("Find Valid Chunks", _context.FindValidChunks);
            _context.OpenConnections = EditorGUILayout.Toggle("Open Connections", _context.OpenConnections);

            //Algorithm stack
            GUILayout.Space(20);
            EditorGUILayout.LabelField("Algorithm Stack:", EditorStyles.boldLabel);
            _algorithmStack.DoLayoutList();

            GUILayout.Space(20);
            EditorGUILayout.LabelField("Size Settings:", EditorStyles.boldLabel);
            _context.GridSize = EditorGUILayout.Vector2IntField("Grid", _context.GridSize);
            _context.ChunkSize = EditorGUILayout.Vector2IntField("Chunk", _context.ChunkSize);

            GUILayout.Space(20);
            EditorGUILayout.LabelField("Chunk Conditions:", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_whitelistedChunks, true);
            EditorGUILayout.PropertyField(_blacklistedChunks, true);

            EditorUtility.SetDirty(_context);
            serializedObject.ApplyModifiedProperties();
        }

        public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
        {
            List<T> assets = new List<T>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }
    }
}