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
        //List of the chunkholder prefabs, that needs a chunk
        private List<ChunkHolder> _chunksToReplace = new List<ChunkHolder>();

        public override bool Process(Map map, List<Chunk> usableChunks)
        {
            //Reset the list before use
            _chunksToReplace.Clear();

            FindChunksToReplace(map);
            PlaceMatchingChunks(map, usableChunks);
            return base.Process(map, usableChunks);
        }

        /// <summary>
        /// Finds all the chunkholders that needs a prefab
        /// </summary>
        /// <param name="map">The map to be used</param>
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

        /// <summary>
        /// Places chunks in the chunkholders, whith a matching cunk type
        /// </summary>
        /// <param name="map">The map to be used</param>
        /// <param name="usableChunks">All chunks or the chunks form the White List</param>
        private void PlaceMatchingChunks(Map map, List<Chunk> usableChunks)
        {
            List<Chunk> listToCheck = usableChunks.Where(chunk => !chunk.ConditionalChunk).ToList();

            foreach (ChunkHolder chunkholder in _chunksToReplace)
            {
                //If the chunkholder that has been found is of the type start, place a start chunk there
                if (chunkholder == map.StartChunk)
                {
                    map.Place(chunkholder,
                        listToCheck.RandomEntry(chunk =>
                            chunkholder.ChunkOpenings.IsMatching(chunk.ChunkOpenings) &&
                            chunk.ChunkType == ChunkType.Start, map.Random));
                    continue;
                }

                //If the chunkholder that has been found is of the type end, place an end chunk there
                if (chunkholder == map.EndChunk)
                {
                    map.Place(chunkholder,
                        listToCheck.RandomEntry(chunk => 
                            chunkholder.ChunkOpenings.IsMatching(chunk.ChunkOpenings) &&
                            chunk.ChunkType == ChunkType.End, map.Random));
                    continue;
                }

                Chunk placeCandidate;

                //If there are any candidates that match the chunkholder type chose them instead.
                if (listToCheck.Any(chunk => chunk.ChunkType == chunkholder.ChunkType))
                {
                    placeCandidate = listToCheck.RandomEntry(chunk =>
                        chunkholder.ChunkOpenings.IsMatching(chunk.ChunkOpenings) &&
                        chunk.ChunkType == chunkholder.ChunkType, map.Random);
                }
                else
                {
                    //If we dident find any of the specific type, find a default one.
                    placeCandidate = listToCheck.RandomEntry(chunk =>
                            chunkholder.ChunkOpenings.IsMatching(chunk.ChunkOpenings) &&
                            chunk.ChunkType == ChunkType.Default, map.Random);
                }

                //Places the actual prefab in the map
                map.Place(chunkholder, placeCandidate);
            }

            if (map.MapBlueprint.FillEmptySpaces)
            {
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
}