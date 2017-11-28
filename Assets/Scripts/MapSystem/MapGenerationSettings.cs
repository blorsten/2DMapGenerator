using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// Purpose: Withholds all the settings for the map generation system.
    /// Creator: MP
    /// </summary>
    public class MapGenerationSettings : ScriptableObject 
    {
        [Header("Gizmo Settings:")]
        [SerializeField] private Color _defaultConnectionColor;
        [SerializeField] private Color _criticalConnectionColor;

        public Color DefaultConnectionColor
        {
            get { return _defaultConnectionColor; }
            set { _defaultConnectionColor = value; }
        }

        public Color CriticalConnectionColor
        {
            get { return _criticalConnectionColor; }
            set { _criticalConnectionColor = value; }
        }
    }
}