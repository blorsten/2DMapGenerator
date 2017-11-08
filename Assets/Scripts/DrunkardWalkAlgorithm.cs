using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// Purpose:
    /// make a path for the MapBuilder to fill with chunks
    /// Creator:
    /// Niels Justesen
    /// </summary>

    [CreateAssetMenu(fileName = "Drunkard Walk Algorithm", menuName = "MapGeneration/Algorithms/DrunkardWalk")]
    public class DrunkardWalkAlgorithm : MapGenerationAlgorithm
    {
        public override void Process(Map map, List<Chunk> usableChunks)
        {
        }
    }
}