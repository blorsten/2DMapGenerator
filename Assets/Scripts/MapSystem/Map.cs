using System;
using System.Collections.Generic;
using System.Linq;
using MapGeneration.Algorithm;
using Random = System.Random;
using MapGeneration.Extensions;
using MapGeneration.SaveSystem;
using MapGeneration.ConditionalChunks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapGeneration
{
    /// <summary>
    /// Purpose: Holds all data and behaviour of a map.
    /// Creator: PW
    /// </summary>
    public class Map : MonoBehaviour
    {
        private ChunkHolder _startChunk;
        private ChunkHolder _endChunk;

        [SerializeField] private int _seed;
        [SerializeField] private MapBlueprint _mapBlueprint;

        public Guid ID { get; private set; }
        public Random Random { get; private set; }
        public ChunkHolder[,] Grid { get; set; }

        public MapDataSaver MapDataSaver { get; set; }

        public int Seed { get { return _seed; } private set { _seed = value; } }
        public MapBlueprint MapBlueprint { get { return _mapBlueprint; } private set { _mapBlueprint = value; } }
        public Dictionary<GameObject,Chunk> Tilemaps = new Dictionary<GameObject, Chunk>();

        public ChunkHolder StartChunk
        {
            get { return _startChunk; }
            set
            {
                if (_startChunk != null)
                    _startChunk.ChunkType = ChunkType.Default;

                _startChunk = value;

                if (_startChunk != null)
                    _startChunk.ChunkType = ChunkType.Start;
            }
        }

        public ChunkHolder EndChunk
        {
            get { return _endChunk; }
            set
            {
                if (_endChunk != null)
                    _endChunk.ChunkType = ChunkType.Default;

                _endChunk = value;

                if (_endChunk != null)
                    _endChunk.ChunkType = ChunkType.End;
            }
        }

        /// <summary>
        /// This initializes the map
        /// </summary>
        /// <param name="seed">The maps seed</param>
        /// <param name="mapBlueprint">The maps blueprint</param>
        /// <param name="random">The random class used</param>
        /// <param name="mapDataSaver">Existing map data saver if any.</param>
        public void Initialize(int seed, MapBlueprint mapBlueprint, MapDataSaver mapDataSaver = null)
        {
            Seed = seed;
            MapBlueprint = mapBlueprint;
            Random = new Random(seed);

            //Generate the map ID from the newly created random.
            ID = new Guid(RandomExtension.GenerateByteSeed(Random));

            Grid = new ChunkHolder[mapBlueprint.GridSize.x,mapBlueprint.GridSize.y];
            for (int x = 0; x < Grid.GetLength(0); x++)
            {
                for (int y = 0; y < Grid.GetLength(1); y++)
                {
                    Grid[x,y] = new ChunkHolder(new Vector2Int(x,y));
                }
            }

            MapDataSaver = mapDataSaver ?? new MapDataSaver(this);
        }

        /// <summary>
        /// Paces a chunk in a chunkholder if it validates
        /// </summary>
        /// <param name="chunkHolder">Where to place</param>
        /// <param name="chunk">Chunk to place</param>
        /// <returns>Wheter the chunk was placed or not</returns>
        public bool Place(ChunkHolder chunkHolder, Chunk chunk, bool forcePlace = false)
        {
            if (chunkHolder.Prefab != null && !forcePlace || chunk == null)
                return false;

            if (chunk.ConditionalChunk)
            {
                if (chunk.ConditionalChunk.Validate(this, chunkHolder))
                {
                    chunkHolder.Prefab = chunk;
                    chunkHolder.ChunkType = chunk.ChunkType;
                    return true;
                }

                return false;
            }

            chunkHolder.Prefab = chunk;
            chunk.Map = this;
            return true;
        }

        /// <summary>
        /// Gets the position of a chunkholder in the grid
        /// </summary>
        /// <param name="chunkHolder">Chunkholder</param>
        /// <returns>Position</returns>
        public Vector2Int GetChunkPos(ChunkHolder chunkHolder)
        {
            for (int x = 0; x < Grid.GetLength(0); x++)
            {
                for (int y = 0; y < Grid.GetLength(1); y++)
                {
                    if(Grid[x,y] == chunkHolder)
                        return new Vector2Int(x, y);
                }
            }
            throw new ArgumentOutOfRangeException("chunkHolder", "Chunkholder not in grid.");
        }

        /// <summary>
        /// Gets all the neighboring chunks
        /// </summary>
        /// <param name="chunkHolder">Chunkholder</param>
        /// <returns>array with neighbors</returns>
        public List<ChunkHolder> GetNeighbor(ChunkHolder chunkHolder)
        {
            return GetNeighbor(chunkHolder, 
                PathAlgorithm.CardinalDirections.Top |
                PathAlgorithm.CardinalDirections.Bottom |
                PathAlgorithm.CardinalDirections.Right |
                PathAlgorithm.CardinalDirections.Left);
        }

        /// <summary>
        /// Gets the neighbors in the given directions
        /// </summary>
        /// <param name="chunkHolder">Chunkholder</param>
        /// <param name="directions">Directions to get</param>
        /// <returns>array with neighbors</returns>
        public List<ChunkHolder> GetNeighbor(ChunkHolder chunkHolder, PathAlgorithm.CardinalDirections directions)
        {
            List<ChunkHolder> listToReturn = new List<ChunkHolder>();
            bool done = false;

            for (int x = 0; x < Grid.GetLength(0); x++)
            {
                for (int y = 0; y < Grid.GetLength(1); y++)
                {
                    if (Grid[x, y] == chunkHolder)
                    {
                        if(directions == PathAlgorithm.CardinalDirections.Top &&
                            y + 1 < Grid.GetLength(1))
                            listToReturn.Add(Grid[x,y+1]);

                        if(directions == PathAlgorithm.CardinalDirections.Bottom &&
                            y - 1 > -1)
                            listToReturn.Add(Grid[x,y-1]);

                        if(directions == PathAlgorithm.CardinalDirections.Right &&
                            x - 1 > -1)
                            listToReturn.Add(Grid[x -1, y]);

                        if (directions == PathAlgorithm.CardinalDirections.Left &&
                            x +1 < Grid.GetLength(0))
                            listToReturn.Add(Grid[x +1, y]);

                        done = true;
                    }

                    if (done)
                        break;
                }

                if (done)
                    break;
            }

            return listToReturn;
        }

        /// <summary>
        /// This draws the map's grid.
        /// </summary>
        public void OnDrawGizmos()
        {
            Vector2 gridSize = MapBlueprint.GridSize;
            Vector2 chunkSize = MapBlueprint.ChunkSize;
            
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {

                    float xPosition = transform.position.x + chunkSize.x * x + chunkSize.x / 2;
                    float yPosition = transform.position.y + chunkSize.y * y + chunkSize.y / 2;

                    Gizmos.DrawWireCube(new Vector3(xPosition,yPosition), chunkSize);
                }
            }
        }

        public ChunkHolder GetChunkHolder(Vector2Int position)
        {
            if (Grid == null)
                return null;
            if ((position.x < 0 || position.x >= Grid.GetLength(0)) ||
                (position.y < 0 || position.y >= Grid.GetLength(1)))
                return null;
            return Grid[position.x, position.y];
        }
    }
}