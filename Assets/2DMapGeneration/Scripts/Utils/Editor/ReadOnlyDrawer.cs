using MapGeneration.Utils;
using UnityEditor;
using UnityEngine;

namespace MapGeneration.Editor.Utils
{
    /// <summary>
    /// Autor: SwissArmyLib - Archon Interactive
    /// Taken from: https://github.com/ArchonInteractive/SwissArmyLib
    /// Direct Link: https://raw.githubusercontent.com/ArchonInteractive/SwissArmyLib/9217938cd7ffbd689cb0ecc1c4b4a6715a84dcf0/Archon.SwissArmyLib.Editor/Utils/ReadOnlyDrawer.cs
    /// Makes fields marked with <see cref="ReadOnlyAttribute"/> uninteractable via the inspector.
    /// </summary>
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        /// <inheritdoc />
        public override void OnGUI(Rect position,
            SerializedProperty property,
            GUIContent label)
        {
            var readOnly = (ReadOnlyAttribute)attribute;

            if (readOnly.OnlyWhilePlaying && !Application.isPlaying)
                EditorGUI.PropertyField(position, property, label, true);
            else
            {
                GUI.enabled = false;
                EditorGUI.PropertyField(position, property, label, true);
                GUI.enabled = true;
            }
        }
    }
}