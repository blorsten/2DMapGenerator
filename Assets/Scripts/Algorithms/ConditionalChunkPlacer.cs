using System.Collections.Generic;
using ListExstention;
using MapGeneration;
using MapGeneration.Algorithm;
using MapGeneration.ConditionalChunks;
using UnityEngine;

namespace CondtionalChunkPlacer
{
    [CreateAssetMenu(fileName = "New Condtional Chunk Placer", menuName = "MapGeneration/Algorithms/Condional Chunk Placer")]
    public class ConditionalChunkPlacer : MapGenerationAlgorithm
    {
        [SerializeField] private int _amountOfChunks = 1;

        public override bool Process(Map map, List<Chunk> usableChunks)
        {
            int iterations = 0;

            foreach (ChunkHolder chunkHolder in map.Grid)
            {
                if (iterations >= _amountOfChunks)
                    break;

                if (chunkHolder.ChunkType == ChunkType.End || chunkHolder.ChunkType == ChunkType.Start)
                    continue;

                if (chunkHolder.ChunkOpenings.IsEmpty())
                    continue;

                if (map.Place(chunkHolder,
                    usableChunks.RandomEntry(chunk =>
                        chunk is ConditionalChunk &&
                        chunkHolder.ChunkOpenings.IsMatching(chunk.ChunkOpenings), map.Random), true))
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