using UnityEngine;

namespace MapGeneration.Utils
{
    /// <summary>
    /// Autor: SwissArmyLib - Archon Interactive
    /// Taken from: https://github.com/ArchonInteractive/SwissArmyLib
    /// Direct Link: https://raw.githubusercontent.com/ArchonInteractive/SwissArmyLib/9217938cd7ffbd689cb0ecc1c4b4a6715a84dcf0/Archon.SwissArmyLib/Utils/Editor/ReadOnlyAttribute.cs
    /// Marks the field to be unchangable via the inspector.
    /// </summary>
    public class ReadOnlyAttribute : PropertyAttribute
    {
        /// <summary>
        /// Whether it should only be readonly during play mode.
        /// </summary>
        public bool OnlyWhilePlaying;
    }
}