using System.Collections.Generic;
using ListExstention;
using MapGeneration;
using MapGeneration.Algorithm;
using MapGeneration.ConditionalChunks;
using UnityEngine;

namespace CondtionalChunkPlacer
{
    [CreateAssetMenu(fileName = "New Condtional Chunk Placer", menuName = "2D Map Generation/Algorithms/Condional Chunk Placer")]
    public class ConditionalChunkPlacer : MapGenerationAlgorithm
    {
        [SerializeField] private int _amountOfChunks = 1;

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