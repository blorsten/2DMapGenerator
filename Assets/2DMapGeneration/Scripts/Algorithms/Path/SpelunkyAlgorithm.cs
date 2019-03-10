using System.Collections.Generic;
using System.Linq;
using MapGeneration.ChunkSystem;
using MapGeneration.Extensions;
using UnityEngine;

namespace MapGeneration.Algorithm
{
    /// <summary>
    /// Makes a path similar to spenlunky's algorithm, with a critical path, that is always open
    /// </summary>
    [CreateAssetMenu(fileName = "New Spelunky Walk", menuName = "2D Map Generation/Algorithms/Spelunky Walk")]
    public class SpelunkyAlgorithm : PathAlgorithm
    {
        /// <summary>
        /// Sets a path from the top, that goes to the bottom of the map, when one bottom side wall is reached the job is done
        /// </summary>
        /// <param name="map"></param>
        /// <param name="usableChunks"></param>
        /// <returns></returns>
        public override bool Process(Map map, List<Chunk> usableChunks)
        {
            //set the startpoint in the top row of the map grid
            Vector2Int startPoint = new Vector2Int(map.Random.Range(0, map.MapBlueprint.GridSize.x), map.MapBlueprint.GridSize.y - 1);

            //The first chunk is marked.
            ChunkHolder firstChunk = map.Grid[startPoint.x, startPoint.y];
            MarkedChunks.Enqueue(firstChunk);

            //map start chunk set
            map.StartChunk = firstChunk;
            Road.Enqueue(new KeyValuePair<ChunkHolder, CardinalDirections?>(firstChunk, null));

            Vector2Int currentPos = startPoint;

            //Establish candidates for directions to take
            ResetDirectionCandidates();

            DirectionCandidates.Remove(CardinalDirections.Top);

            //If there are no more candidates, the work is done.
            while (DirectionCandidates.Count > 0)
            {
                var nextChunk = FindNextChunk(map, usableChunks, ref currentPos);

                if (nextChunk != null)
                {
                    Road.Enqueue(nextChunk.GetValueOrDefault());
                    DirectionCandidates.Remove(CardinalDirections.Top);
                }
            }

            if (Road == null || Road != null && !Road.Any())
                return false;

            BackTrackChunks(Road, ChunkOpenings.ConnectionType.Critical);
            return true;
        }
    }
}