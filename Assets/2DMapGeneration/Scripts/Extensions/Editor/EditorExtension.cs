using UnityEditor;
using UnityEngine;

namespace MapGeneration.Extensions
{
    /// <summary>
    /// An extension for the unity editor.
    /// </summary>
    public static class EditorExtension
    {
        /// <summary>
        /// Converts an object to a serialized object and tries to draw its own fields/properties.
        /// </summary>
        public static void DrawSerializedProperty(SerializedProperty obj)
        {
            if (obj != null && obj.objectReferenceValue != null)
            {
                SerializedObject serializedAlgorithm = new SerializedObject(obj.objectReferenceValue);

                bool hasDrawnHeader = false;

                serializedAlgorithm.Update();

                var prop = serializedAlgorithm.GetIterator();
                prop.NextVisible(true);

                while (prop.NextVisible(true))
                {
                    if (!hasDrawnHeader)
                    {
                        EditorGUILayout.LabelField(string.Format("{0} Settings:", obj.name), EditorStyles.boldLabel);
                        hasDrawnHeader = true;
                    }

                    EditorGUILayout.PropertyField(prop);
                }

                if (GUI.changed)
                    serializedAlgorithm.ApplyModifiedProperties();
            }
        }
    }
}