using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapGeneration.Algorithm
{
    /// <summary>
    /// Purpose: 
    /// Set biome on the generated map with diamond square
    /// Creator:
    /// Niels Justesen
    /// </summary>
    [CreateAssetMenu(fileName = "New Diamond Square", menuName = "MapGeneration/Algorithms/Diamond Square Biome")]
    public class DiamondSquareBiome : MapGenerationAlgorithm
    {
        private float[,] _noiseGrid;
        private int _width;
        private int _heigt;

        public override bool Process(Map map, List<Chunk> usableChunks)
        {
            _width = map.MapBlueprint.GridSize.x;
            _noiseGrid = new float[_width, _heigt];
            return base.Process(map, usableChunks);
        }
    }
}