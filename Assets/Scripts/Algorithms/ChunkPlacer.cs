using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using ListExstention;
using UnityEditor;

namespace MapGeneration.Algorithm
{
    /// <summary>
    /// Purpose:
    /// find all chunks and replace them with a playable inviroment.
    /// Creator:
    /// Niels Justesen
    /// </summary>

    [CreateAssetMenu(fileName = "New Chunk Placer", menuName = "MapGeneration/Algorithms/Chunk Placer")]
    public class ChunkPlacer : MapGenerationAlgorithm
    {
        private List<ChunkHolder> _chunksToReplace = new List<ChunkHolder>();

        public override bool Process(Map map, List<Chunk> usableChunks)
        {
            _chunksToReplace.Clear();

            FindChunksToReplace(map);
            PlaceMatchingChunks(map, usableChunks);
            return base.Process(map, usableChunks);
        }

        private void FindChunksToReplace(Map map)
        {
            foreach (ChunkHolder chunk in map.Grid)
            {
                if (!chunk.ChunkOpenings.IsEmpty())
                {
                    _chunksToReplace.Add(chunk);
                }
            }
        }

        private void PlaceMatchingChunks(Map map, List<Chunk> usableChunks)
        {
            foreach (ChunkHolder chunkholder in _chunksToReplace)
            {
                if (chunkholder == map.StartChunk)
                {
                    map.Place(chunkholder,
                        usableChunks.RandomEntry(chunk => chunkholder.ChunkOpenings == chunk.ChunkOpenings &&
                                                          chunk.ChunkType == ChunkType.Start));
                    continue;
                }

                if (chunkholder == map.EndChunk)
                {
                    map.Place(chunkholder,
                        usableChunks.RandomEntry(chunk => chunkholder.ChunkOpenings == chunk.ChunkOpenings &&
                                                          chunk.ChunkType == ChunkType.End));
                    continue;
                }

                map.Place(chunkholder,
                    usableChunks.RandomEntry(chunk => chunkholder.ChunkOpenings == chunk.ChunkOpenings &&
                                                      chunk.ChunkType == ChunkType.Default));
            }
        }
    }
}