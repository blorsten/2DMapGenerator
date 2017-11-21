using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace MapGeneration.Algorithm
{
    /// <summary>
    /// Purpose:
    /// find all chunks and replace them with a playable inviroment.
    /// Creator:
    /// Niels Justesen
    /// </summary>

    [CreateAssetMenu(fileName = "Set Valid Chunks", menuName = "MapGeneration/Algorithms/FindAndReplaceChunks")]
    public class FindAndReplaceChunks : MapGenerationAlgorithm
    {
        private List<ChunkHolder> _chunksToReplace = new List<ChunkHolder>();
        public override void Process(Map map, List<Chunk> usableChunks)
        {
            _chunksToReplace.Clear();
            foreach (ChunkHolder chunk in map.Grid)
            {
                if (chunk.Prefab != null)
                {
                    _chunksToReplace.Add(chunk);
                }
            }



            base.Process(map, usableChunks);
        }

        public override void PostProcess(Map map, List<Chunk> usableChunks)
        {
            PlaceMatchingChunks(map, usableChunks);
            base.PostProcess(map, usableChunks);
        }

        private void PlaceMatchingChunks(Map map, List<Chunk> usableChunks)
        {
            foreach (ChunkHolder chunkholder in _chunksToReplace)
            {
                foreach (Chunk chunk in usableChunks)
                {
                    if (chunkholder.Prefab.ChunkOpenings == chunk.ChunkOpenings)
                    {
                        map.Place(chunkholder.Prefab, chunk);
                    }
                }
            }
        }
    }
}