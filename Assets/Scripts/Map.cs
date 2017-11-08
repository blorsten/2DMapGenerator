using System;
using Random = System.Random;
using Extensions;

namespace MapGeneration
{
    /// <summary>
    /// Purpose:
    /// Creator:
    /// </summary>
    public class Map 
    {
        public Guid ID { get; private set; }
        public Random Random { get; private set; }
        public int Seed { get; private set; }
        public MapBlueprint MapBlueprint { get; private set; }
        public ChunkHolder[,] Grid { get; set; }
        public ChunkHolder StartChunk { get; set; }
        public ChunkHolder EndChunk { get; set; }

        public Map(int seed, MapBlueprint mapBlueprint, Random random)
        {
            Seed = seed;
            MapBlueprint = mapBlueprint;
            Random = random;

            //Generate the map ID from the newly created random.
            ID = new Guid(RandomExtension.GenerateByteSeed(Random));
        }
    }
}