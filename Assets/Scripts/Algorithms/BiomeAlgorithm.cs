using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration.Algorithm
{
    /// <summary>
    /// Purpose: 
    /// Set biome on the generated map
    /// Creator:
    /// Niels Justesen
    /// </summary>
    [CreateAssetMenu(fileName = "New Biome", menuName = "MapGeneration/Algorithms/Biome")]
    public class BiomeAlgorithm : MapGenerationAlgorithm
    {
        private float[,] _noiseGrid;
        private int _width;
        private int _heigt;
        private int _scale = 20;

        public override bool Process(Map map, List<Chunk> usableChunks)
        {
            _width = map.Grid.GetLength(0) * map.MapBlueprint.ChunkSize.x;
            _heigt = map.Grid.GetLength(1) * map.MapBlueprint.ChunkSize.y;

            _noiseGrid = new float[_width, _heigt];

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _heigt; y++)
                {
                    _noiseGrid[x, y] = CalculateNoise(x, y);
                }
            }

            return base.Process(map, usableChunks);
        }

        public override bool PostProcess(Map map, List<Chunk> usableChunks)
        {
            foreach (ChunkHolder chunk in map.Grid)
            {
                for (int x = 0; x < _width; x++)
                {
                    for (int y = 0; y < _heigt; y++)
                    {
                        chunk.Prefab.Enviorment.color = new Color(_noiseGrid[x, y], _noiseGrid[x, y], _noiseGrid[x, y]);
                    }
                }
            }
            return base.PostProcess(map, usableChunks);
        }

        private float CalculateNoise(int x, int y)
        {
            float xCoord = (float)x / _width * _scale;
            float yCoord = (float)y / _heigt * _scale;
            return Mathf.PerlinNoise(xCoord, yCoord);
        }
    }
}