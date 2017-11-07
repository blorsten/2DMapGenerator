using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// Purpose:
    /// Creator:
    /// </summary>
    public abstract class MapGenerationAlgorithm : ScriptableObject
    {
        /// <summary>
        /// Process a given map
        /// </summary>
        /// <param name="map">map to process</param>
        /// <param name="mapBlueprint">related blueprint</param>
        public abstract void Process(Map map, MapBlueprint mapBlueprint);
    }
}