using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using ListExstention;
using MapGeneration.ConditionalChunks;

namespace MapGeneration.Algorithm
{
    /// <summary>
    /// Purpose:
    /// find all chunks and replace them with a playable inviroment.
    /// Creator:
    /// Niels Justesen
    /// </summary>
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
            List<Chunk> listToCheck = usableChunks.Where(chunk => !chunk.ConditionalChunk).ToList();

            foreach (ChunkHolder chunkholder in _chunksToReplace)
            {
                if (chunkholder == map.StartChunk)
                {
                    map.Place(chunkholder,
                        listToCheck.RandomEntry(chunk =>
                            chunkholder.ChunkOpenings.IsMatching(chunk.ChunkOpenings) &&
                            chunk.ChunkType == ChunkType.Start, map.Random));
                    continue;
                }

                if (chunkholder == map.EndChunk)
                {
                    map.Place(chunkholder,
                        listToCheck.RandomEntry(chunk => 
                            chunkholder.ChunkOpenings.IsMatching(chunk.ChunkOpenings) &&
                            chunk.ChunkType == ChunkType.End, map.Random));
                    continue;
                }

                map.Place(chunkholder,
                    listToCheck.RandomEntry(chunk => 
                        chunkholder.ChunkOpenings.IsMatching(chunk.ChunkOpenings) &&
                        chunk.ChunkType == ChunkType.Default, map.Random));
            }


            //Fills all voids in the map with solid chunks.
            foreach (ChunkHolder holder in map.Grid)
            {
                if (holder.Prefab == null)
                {
                    map.Place(holder, listToCheck.RandomEntry(chunk => chunk.ChunkType == ChunkType.Solid, map.Random));
                }
            }
        }
    }
}