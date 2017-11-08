using System.Collections.Generic;
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
        /// <param name="usableChunks">List of all the usable chunks, filtered by <see cref="MapBlueprint.Generate"/>.</param>
        public abstract void Process(Map map, List<Chunk> usableChunks);
    }
}