using System.Collections.Generic;
using MapGeneration.Algorithm;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;
using MapGeneration.Extensions;

namespace MapGeneration.Editor
{
    /// <summary>
    /// Purpose: Draws a custom inspector for map blueprints.
    /// Creator: MP
    /// </summary>
    [CustomEditor(typeof(MapBlueprint))]
    public class MapBlueprintEditor : UnityEditor.Editor
    {
        private MapBlueprint _context;
        private SerializedProperty _whitelistedChunks;
        private SerializedProperty _blacklistedChunks;

        private ReorderableAlgorithmStack _algorithmStack;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            //Auto settings
            EditorGUILayout.LabelField("Automation Settings:", EditorStyles.boldLabel);
            _context.FillEmptySpaces = EditorGUILayout.Toggle("Fill Empty Spaces", _context.FillEmptySpaces);
            _context.FindValidChunks = EditorGUILayout.Toggle("Find Valid Chunks", _context.FindValidChunks);
            _context.OpenConnections = EditorGUILayout.Toggle("Open Openings", _context.OpenConnections);

            //Algorithm stack
            if (_algorithmStack != null)
            {
                GUILayout.Space(20);
                EditorGUILayout.LabelField("Algorithm Stack:", EditorStyles.boldLabel);
                _algorithmStack.List.DoLayoutList();
                _algorithmStack.DrawSelectedAlgorithm();
            }
             
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

        private void OnEnable()
        {
            _context = target as MapBlueprint;
            _whitelistedChunks = serializedObject.FindProperty("WhitelistedChunks"); 
            _blacklistedChunks = serializedObject.FindProperty("BlacklistedChunks");
            _algorithmStack = new ReorderableAlgorithmStack(serializedObject, serializedObject.FindProperty("AlgorithmStack"), _context.AlgorithmStack);
        }

        public class ReorderableAlgorithmStack
        {
            public SerializedProperty SelectedElement { get; set; }
            public ReorderableList List { get; set; }

            public ReorderableAlgorithmStack(SerializedObject serializedObject, SerializedProperty serializedProperty, List<AlgorithmStorage> stack, bool hideFooter = false)
            {
                //Instantiate a new reorderablelist
                List = new ReorderableList(
                    serializedObject,
                    serializedProperty,
                    true, false, true, true);
                
                //We need to hook some methods to events on the new reorderable list.
                List.onAddDropdownCallback = (rect, list) =>
                {
                    var menu = new GenericMenu(); //Create a clean context menu

                    //Lets find all the assets under the type MapGenerationAlgorithm.
                    var algorithms = AssetDatabaseExtension.FindAssetsByType<MapGenerationAlgorithm>();

                    //Add each of the found algorithms to the newly made context menu.
                    foreach (var algorithm in algorithms)
                    {
                        menu.AddItem(new GUIContent(algorithm.name), false, data =>
                        {
                            stack.Add(new AlgorithmStorage(data as MapGenerationAlgorithm));
                        }, algorithm);
                    }

                    menu.ShowAsContext();
                };

                List.drawElementCallback = (rect, index, active, focused) =>
                {
                    //Lets create some offset for the elements.
                    rect.y += 2;

                    //First we grab an element from the algorithm stack
                    var targetObject = List.serializedProperty.GetArrayElementAtIndex(index);

                    var algorithmValue = targetObject.FindPropertyRelative("Algorithm");
                    var toggleValue = targetObject.FindPropertyRelative("IsActive");

                    float toggleWidth = 15;

                    //Then we create an object field for the object.
                    if (hideFooter)
                        GUI.enabled = false;

                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width - (toggleWidth + 5), EditorGUIUtility.singleLineHeight),
                        algorithmValue, GUIContent.none);

                    if (hideFooter)
                        GUI.enabled = true;

                    EditorGUI.PropertyField(new Rect(rect.width + toggleWidth, rect.y, toggleWidth, EditorGUIUtility.singleLineHeight),
                        toggleValue, GUIContent.none);

                    //If this element is active we make it the current active algorithm.
                    if (active)
                        SelectedElement = algorithmValue;

                    //If this element is in focus and active we make it go away with a key press.
                    if (!hideFooter && focused && active && Event.current.keyCode == KeyCode.Delete && Event.current.type == EventType.KeyDown)
                    {
                        stack.RemoveAt(index);
                        if (SelectedElement == algorithmValue)
                            SelectedElement = null;
                    }
                };

                if (hideFooter)
                    List.drawFooterCallback += rect => { };
            }

            public void DrawSelectedAlgorithm()
            {
                GUILayout.Space(10);
                EditorExtension.DrawSerializedProperty(SelectedElement);
            }
        }
    }
}