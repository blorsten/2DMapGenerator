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

        public virtual void OnDrawGizmos()
        {

            for (int x = 0; x < MapBlueprint.GridSize.x; x++)
            {
                for (int y = 0; y < MapBlueprint.GridSize.y; y++)
                {
                    float xPosition = x;
                    float yPosition = y;

                    float xOffset = Size.x * (_cellGap.x + _cellSize.x) / 2 - _cellSize.x / 2;
                    float yOffset = Size.y * (_cellGap.y + _cellSize.y) / 2 - _cellSize.y / 2;

                    Vector2 point = new Vector2(
                        transform.position.x + xPosition - xOffset,
                        transform.position.y + yPosition - yOffset);

                    Gizmos.DrawWireCube(point, _cellSize);
                }
            }
        }
    }
}