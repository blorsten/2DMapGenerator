using System;
using Random = System.Random;
using Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

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
    }
}