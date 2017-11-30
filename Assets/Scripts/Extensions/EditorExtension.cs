using UnityEditor;
using UnityEngine;

namespace MapGeneration.Extensions
{
    public static class EditorExtension
    {
        /// <summary>
        /// Convers an object to a serialized object and tries to draw its own fields/properties.
        /// </summary>
        public static void DrawSerializedObject(Object obj)
        {
            if (obj != null)
            {
                SerializedObject serializedAlgorithm = new SerializedObject(obj);

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