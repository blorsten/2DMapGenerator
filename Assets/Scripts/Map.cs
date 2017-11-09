using System;
using Random = System.Random;
using MapGeneration.Extensions;
using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// Purpose:
    /// Creator:
    /// </summary>
    public class Map : MonoBehaviour
    {
        public Guid ID { get; private set; }
        public Random Random { get; private set; }
        public int Seed { get; private set; }
        public MapBlueprint MapBlueprint { get; private set; }
        public ChunkHolder[,] Grid { get; set; }
        public ChunkHolder StartChunk { get; set; }
        public ChunkHolder EndChunk { get; set; }

        /// <summary>
        /// This initializes the map
        /// </summary>
        /// <param name="seed">The maps seed</param>
        /// <param name="mapBlueprint">The maps blueprint</param>
        /// <param name="random">The random class used</param>
        public void Initialize(int seed, MapBlueprint mapBlueprint, Random random)
        {
            Seed = seed;
            MapBlueprint = mapBlueprint;
            Random = random;
            
            //Generate the map ID from the newly created random.
            ID = new Guid(RandomExtension.GenerateByteSeed(Random));

            Grid = new ChunkHolder[mapBlueprint.GridSize.x,mapBlueprint.GridSize.y];
            for (int x = 0; x < Grid.GetLength(0); x++)
            {
                for (int y = 0; y < Grid.GetLength(1); y++)
                {
                    Grid[x,y] = new ChunkHolder();
                }
            }
        }

        /// <summary>
        /// This draws the map's grid.
        /// </summary>
        public void OnDrawGizmos()
        {
            for (int x = 0; x < Grid.GetLength(0); x++)
            {
                for (int y = 0; y < Grid.GetLength(1); y++)
                {
                    float xPosition = transform.position.x;
                    float yPosition = transform.position.y;

                    Vector2 chunkSize = new Vector2(MapBlueprint.ChunkSize.x, MapBlueprint.ChunkSize.y);

                    Vector2 currentPosition = new Vector2(
                        xPosition + MapBlueprint.ChunkSize.x * x,
                        yPosition + MapBlueprint.ChunkSize.y * y);

                    Gizmos.DrawWireCube(currentPosition, chunkSize);
                }
            }
        }
    }
}