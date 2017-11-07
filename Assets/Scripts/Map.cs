using System;
using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// Purpose:
    /// Creator:
    /// </summary>
    public class Map 
    {
        public Guid ID { get; private set; }
        public System.Random Random { get; private set; }
        public  int Seed { get; private set; }
        public MapBlueprint MapBlueprint { get; private set; }
        public ChunkHolder[,] Grid { get; private set; }
        public ChunkHolder StartChunk { get; private set; }
        public ChunkHolder EndChunk { get; private set; }

        public Map(int seed, MapBlueprint mapBlueprint)
        {
            Seed = seed;
            MapBlueprint = mapBlueprint;
        }
    }
}