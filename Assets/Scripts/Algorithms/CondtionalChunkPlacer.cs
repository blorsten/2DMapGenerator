using System.Collections.Generic;
using MapGeneration;
using MapGeneration.Algorithm;
using MapGeneration.ConditionalChunks;
using UnityEngine;

namespace CondtionalChunkPlacer
{
    /// <summary>
    /// Purpose:
    /// Creator:
    /// </summary>
    [CreateAssetMenu(fileName = "New Condtional Chunk Placer", menuName = "MapGeneration/Algorithms/Condional Chunk Placer")]
    public class CondtionalChunkPlacer : MapGenerationAlgorithm
    {
        [SerializeField] private ConditionalChunk _chunkToPlace;

        public override bool Process(Map map, List<Chunk> usableChunks)
        {
            foreach (ChunkHolder chunkHolder in map.Grid)
            {
                if (chunkHolder.ChunkType == ChunkType.End || chunkHolder.ChunkType == ChunkType.Start)
                    continue;

                if (!chunkHolder.Prefab)
                    continue;

                if (map.Place(chunkHolder, _chunkToPlace, true))
                    return true;
            }

            return base.Process(map, usableChunks);
        }
    }
}