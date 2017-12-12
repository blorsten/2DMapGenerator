using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// Keeps all the settings for the map generation system.
    /// </summary>
    public class MapGenerationSettings : ScriptableObject 
    {
        [Header("Gizmo Settings:")]
        [SerializeField] private Color _defaultConnectionColor;
        [SerializeField] private Color _criticalConnectionColor;

        [Header("Biome Setings")]
        [SerializeField] private List<string> _biomes = new List<string>();

        /// <summary>
        /// The default color for the connection path gizmo.
        /// </summary>
        public Color DefaultConnectionColor
        {
            get { return _defaultConnectionColor; }
            set { _defaultConnectionColor = value; }
        }

        /// <summary>
        /// The color for the critical path gizmo.
        /// </summary>
        public Color CriticalConnectionColor
        {
            get { return _criticalConnectionColor; }
            set { _criticalConnectionColor = value; }
        }

        /// <summary>
        /// A list for the different biomes.
        /// </summary>
        public List<string> Biomes
        {
            get { return _biomes; }
            set { _biomes = value; }
        }

        /// <summary>
        /// Call this to translate noise(a number from 0 to 1) to a biome.
        /// </summary>
        /// <param name="noice"></param>
        /// <returns></returns>
        public string NoiseToBiome(float noice)
        { 
            int index = Mathf.CeilToInt(Mathf.Clamp(noice * _biomes.Count - 1, 0, _biomes.Count - 1));
            return _biomes[index];

        }
    }
}