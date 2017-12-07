using MapGeneration.Extensions;
using System.Collections.Generic;
using MapGeneration.ChunkSystem;
using UnityEngine;

namespace MapGeneration.Algorithm
{
    /// <summary>
    /// Purpose: 
    /// Set biome on the generated map with diamond square
    /// Creator:
    /// Niels Justesen og Mikkel Bruun
    /// </summary>
    [CreateAssetMenu(fileName = "New Voronoir Biome", menuName = "2D Map Generation/Algorithms/Voronoi Biome")]
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
            for (int i = 1; i <= _nrOfPoints; i++)
            {
                float noise = 1f / _nrOfPoints * i;

                _noiseGrid[map.Random.Range(0, _width - 1), map.Random.Range(0, _heigt - 1)] = noise;
            }


            List<VornonoiData> reuseableList = new List<VornonoiData>();
            _noiseGrid = VornonoiPopulation(_noiseGrid, ref reuseableList);

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

        private float[,] VornonoiPopulation(float[,] map, ref List<VornonoiData> reuseableList)
        {
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
                                if(map[pos.x,pos.y] == 0 && !reuseableList.Exists(
                                    o => o.Position.x == pos.x && o.Position.y == pos.y))
                                {
                                    valid = true;
                                    reuseableList.Add(new VornonoiData(new Vector2Int(x, y), map[x, y]));
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (reuseableList.Count <= 0)
                return map;

            reuseableList.Sort((x, y) => x.Value.CompareTo(y.Value));

            for (int i = 0; i < reuseableList.Count; i++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        Vector2Int pos = reuseableList[i].Position + new Vector2Int(x, y);
                        if ((x == 0 && y == 0) || pos.x < 0 || pos.y < 0 || pos.x >= _width || pos.y >= _heigt)
                            continue;
                        if (map[pos.x, pos.y] == 0)
                            map[pos.x, pos.y] = reuseableList[i].Value;
                    }
                }
            }
            reuseableList.Clear();
            return VornonoiPopulation(map, ref reuseableList);
        }
    }

}

public struct VornonoiData
{
    public Vector2Int Position { get; set; }
    public float Value { get; set; }

    public VornonoiData(Vector2Int postion, float value) : this()
    {
        Position = postion;
        Value = value;
    }
}