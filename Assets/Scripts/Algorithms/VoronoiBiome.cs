using MapGeneration.Extensions;
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
    [CreateAssetMenu(fileName = "New Voronoir Biome", menuName = "MapGeneration/Algorithms/Voronoi Biome")]
    public class VoronoiBiome : MapGenerationAlgorithm
    {
        [SerializeField, Tooltip("Defines the number of biomes set on the map")] private int _nrOfPoints;
        private float[,] _noiseGrid;
        private int _width;
        private int _heigt;


        public override bool Process(Map map, List<Chunk> usableChunks)
        {
            _width = map.Grid.GetLength(0) * map.MapBlueprint.ChunkSize.x;
            _heigt = map.Grid.GetLength(1) * map.MapBlueprint.ChunkSize.y;
            _noiseGrid = new float[_width, _heigt];

            //Set the random points in the grid
            for (int i = 0; i < _nrOfPoints; i++)
            {
                float noise = map.Random.Range(0f, 1f);
                
                _noiseGrid[map.Random.Range(0, _width-1), map.Random.Range(0, _heigt-1)] = noise;
            }


            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _heigt; y++)
                {
                    if (_noiseGrid[x,y] > 0)
                    {
                        SetNeighborsOfPoint(x, y, _noiseGrid[x, y]);
                    }
                }
            }
            return base.Process(map, usableChunks);
        }

        private void SetNeighborsOfPoint(int x, int y, float noise)
        {
           
        }
    }
}