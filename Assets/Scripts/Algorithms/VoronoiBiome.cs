using MapGeneration.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

namespace MapGeneration.Algorithm
{
    /// <summary>
    /// Purpose: 
    /// Set biome on the generated map with diamond square
    /// Creator:
    /// Niels Justesen og Mikkel Bruun
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

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _heigt; y++)
                {
                    _noiseGrid[x, y] = 0;
                }
            }

            //Set the random points in the grid
            for (int i = 0; i < _nrOfPoints; i++)
            {
                float noise = map.Random.Range(0.01f, 1f);

                _noiseGrid[map.Random.Range(0, _width - 1), map.Random.Range(0, _heigt - 1)] = noise;
            }

            _noiseGrid = VornonoiPopulation(_noiseGrid);

            return base.Process(map, usableChunks);
        }

        public override bool PostProcess(Map map, List<Chunk> usableChunks)
        {
            foreach (ChunkHolder chunk in map.Grid)
            {
                if (chunk.Instance != null)
                {
                    int width = chunk.Instance.Width;
                    int height = chunk.Instance.Height;

                    for (int x = 0; x < chunk.Instance.Width; x++)
                    {
                        for (int y = 0; y < chunk.Instance.Height; y++)
                        {
                            chunk.Instance.BiomeValues[x, y] = _noiseGrid[x + width * chunk.Position.x, y + height * chunk.Position.y];
                        }
                    }
                }
            }
            return base.PostProcess(map, usableChunks);
        }

        private float[,] VornonoiPopulation(float[,] map)
        {
            List<VornonoiData> _dataToTouch = new List<VornonoiData>();
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _heigt; y++)
                {
                    if (map[x, y] > 0)
                    {
                        bool valid = false;
                        for (int i = -1; i <= 1; i++)
                        {
                            if (valid)
                                break;
                            for (int j = -1; j < 1; j++)
                            {
                                Vector2Int pos = new Vector2Int(x + i, y + j);
                                if ((i == 0 && j == 0) || pos.x < 0 || pos.y < 0 || pos.x >= _width || pos.y >= _heigt)
                                    continue;
                                if(map[pos.x,pos.y] == 0)
                                {
                                    valid = true;
                                    _dataToTouch.Add(new VornonoiData(new Vector2Int(x, y), map[x, y]));
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (_dataToTouch.Count <= 0)
                return map;

            _dataToTouch.Sort((x, y) => x.Value.CompareTo(y.Value));

            foreach (var data in _dataToTouch)
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        Vector2Int pos = data.Position + new Vector2Int(x, y);
                        if ((x == 0 && y == 0) || pos.x < 0 || pos.y < 0 || pos.x >= _width || pos.y >= _heigt)
                            continue;
                        if (map[pos.x, pos.y] == 0)
                            map[pos.x, pos.y] = data.Value;
                    }
                }
            }
            _dataToTouch.Clear();
            return VornonoiPopulation(map);
        }
    }

}

public struct VornonoiData
{
    public Vector2Int Position { get; set; }
    public float Value { get; set; }

    public VornonoiData(Vector2Int postion, float value)
    {
        Position = postion;
        Value = value;
    }
}