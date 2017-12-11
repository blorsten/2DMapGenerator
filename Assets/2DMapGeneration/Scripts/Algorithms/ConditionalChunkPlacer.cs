using System.Collections.Generic;
using MapGeneration.ChunkSystem;
using MapGeneration.Extensions;
using UnityEngine;

namespace MapGeneration.Algorithm
{
    /// <summary>
    /// Finds empty chunkholders and tries to place a random conditional chunk that matches.
    /// </summary>
    [CreateAssetMenu(fileName = "New Condtional Chunk Placer", menuName = "2D Map Generation/Algorithms/Condional Chunk Placer")]
    public class ConditionalChunkPlacer : MapGenerationAlgorithm
    {
        [SerializeField] private int _amountOfChunks = 1;

        /// <summary>
        /// Takes <see cref="_amountOfChunks"/> as amount of chunk it can place, and tries to place 
        /// conditional chunks on empty chunkholders until it reaches the maximum.
        /// </summary>
        /// <param name="map">The map to operate on.</param>
        /// <param name="usableChunks">What chunks can the placer use.</param>
        /// <returns>Returns true if the succeeded</returns>
        public override bool Process(Map map, List<Chunk> usableChunks)
        {
            int iterations = 0;

            foreach (ChunkHolder chunkHolder in map.Grid)
            {
                var holder = chunkHolder;

                if (iterations >= _amountOfChunks)
                    break;

                if (holder.ChunkType == ChunkType.End || holder.ChunkType == ChunkType.Start)
                    continue;

                if (holder.ChunkOpenings.IsEmpty())
                    continue;

                if (map.Place(holder,
                    usableChunks.RandomEntry(chunk =>
                        chunk.ConditionalChunk &&
                        holder.ChunkOpenings.IsMatching(chunk.ChunkOpenings), map.Random), true))
                {
                    iterations++;
                }
            }

            if (iterations == 0)
                return false;

            return base.Process(map, usableChunks);
        }
    }
}