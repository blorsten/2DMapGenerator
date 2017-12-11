using MapGeneration.Extensions;
using System.Collections.Generic;
using System.Linq;
using MapGeneration;
using MapGeneration.ChunkSystem;
using UnityEngine;

namespace MapGeneration.Algorithm
{
    /// <summary>
    /// Sets biome on the generated map with diamond square algortihm.
    /// <a href="https://en.wikipedia.org/wiki/Voronoi_diagram">See Source</a>
    /// </summary>
    [CreateAssetMenu(fileName = "New Voronoir Biome", menuName = "2D Map Generation/Algorithms/Voronoi Biome")]
    public class VoronoiBiome : MapGenerationAlgorithm
    {
        [SerializeField, Tooltip("Defines the number of biomes set on the map")] private int _nrOfPoints;
        private float[,] _noiseGrid;
        private int _width;
        private int _heigt;    

        private List<Vector2Int> _directionsToCheck = new List<Vector2Int>()
        {
            new Vector2Int(-1,0),
            new Vector2Int(1,0),
            new Vector2Int(0,-1),
            new Vector2Int(0,1)
        };

        /// <summary>
        /// Starts the process of this algorithm.
        /// </summary>
        /// <param name="map">The map to operate on.</param>
        /// <param name="usableChunks">What chunks can the placer use.</param>
        /// <returns>Returns true if the succeeded</returns>
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
                int x = map.Random.Range(0, _width - 1);
                int y = map.Random.Range(0, _heigt - 1);

                _noiseGrid[x, y] = noise;
            }

            List<VornonoiData> data = new List<VornonoiData>();

            _noiseGrid = VornonoiPopulation(_noiseGrid, ref data);

            return base.Process(map, usableChunks);
        }

        /// <summary>
        /// Goes through all the instantiated chunkholders and sets a biome value to the ones it
        /// calculated in the process phase.
        /// </summary>
        /// <param name="map">The map to operate on.</param>
        /// <param name="usableChunks">What chunks can the placer use.</param>
        /// <returns>Returns true if the succeeded</returns>
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

        private float[,] VornonoiPopulation(float[,] map, ref List<VornonoiData> data)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _heigt; y++)
                {
                    if (map[x, y] > 0)
                    {

                        for (int i = 0; i < _directionsToCheck.Count; i++)
                        {
                            Vector2Int directions = _directionsToCheck[i];
                            Vector2Int pos = new Vector2Int(x + directions.x, y + directions.y);

                            if (pos.x < 0 || pos.y < 0 || pos.x >= _width || pos.y >= _heigt)
                                continue;

                            if (map[pos.x, pos.y] == 0 && !data.Exists(
                                    o => o.Position.x == pos.x && o.Position.y == pos.y))
                            {
                                data.Add(new VornonoiData(new Vector2Int(x, y), map[x, y]));
                                break;
                            }
                        }

                    }
                }
            }

            if (data.Count <= 0)
                return map;

            data.Sort((x, y) => x.Value.CompareTo(y.Value));

            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < _directionsToCheck.Count; j++)
                {
                    Vector2Int directions = _directionsToCheck[j];
                    Vector2Int pos = data[i].Position + directions;

                    if (pos.x < 0 || pos.y < 0 || pos.x >= _width || pos.y >= _heigt)
                        continue;
                    if (map[pos.x, pos.y] == 0)
                    {
                        map[pos.x, pos.y] = data[i].Value;
                    }
                }
            }
            data.Clear();
            return VornonoiPopulation(map, ref data);
        }
    }

    /// <summary>
    /// Holds date of a position and what scalar value it has.
    /// </summary>
    public struct VornonoiData
    {
        /// <summary>
        /// Position in the <see cref="Map.Grid"/>.
        /// </summary>
        public Vector2Int Position { get; set; }

        /// <summary>
        /// A scalar value between 0 and 1 that decides a specific biome.
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        /// Construct a voronoi data object that has a grid position and scalar value.
        /// </summary>
        /// <param name="postion"></param>
        /// <param name="value"></param>
        public VornonoiData(Vector2Int postion, float value) : this()
        {
            Position = postion;
            Value = value;
        }
    }
}