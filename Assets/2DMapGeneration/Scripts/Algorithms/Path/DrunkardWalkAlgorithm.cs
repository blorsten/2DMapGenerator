using System.Collections.Generic;
using System.Linq;
using MapGeneration.ChunkSystem;
using MapGeneration.Extensions;
using UnityEngine;

namespace MapGeneration.Algorithm
{
    /// <summary>
    /// Starts at a random position in the map grid and starts a walk from there.
    /// <a href="http://pcg.wikidot.com/pcg-algorithm:drunkard-walk">See Source</a>
    /// </summary>
    [CreateAssetMenu(fileName = "New Drunkard Walk", menuName = "2D Map Generation/Algorithms/Drunkard Walk")]
    public class DrunkardWalkAlgorithm : PathAlgorithm
    {
        //Number of times the algorithms creates a marked chunk.
        [SerializeField] private int _pathLength = 10;

        /// <summary>
        /// Starts a Drunkard Walk until it reaches a dead end or <see cref="_pathLength"/> is reached.
        /// </summary>
        /// <param name="map">The map to operate on.</param>
        /// <param name="usableChunks">What chunks can the placer use.</param>
        /// <returns>Returns true if the succeeded</returns>
        public override bool Process(Map map, List<Chunk> usableChunks)
        {
            bool success = base.Process(map, usableChunks);

            //This is where the walk starts.
            var chunks = map.Grid.Cast<ChunkHolder>().Where(holder => holder.ChunkOpenings.IsEmpty()).ToList();
            ChunkHolder startChunk = chunks.RandomEntry(map.Random);

            if (startChunk != null)
            {
                //The first chunk is marked.
                StartWalk(map, usableChunks, startChunk.Position);
            }

            if (Road == null || Road != null && !Road.Any())
                return false;

            BackTrackChunks(Road);
            return success;
        }

        /// <summary>
        /// Starts a drunkard walk, picks a random point on the grid and starts its walk.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="usableChunks"></param>
        /// <param name="startPosition"></param>
        /// <returns></returns>
        protected virtual Queue<KeyValuePair<ChunkHolder, CardinalDirections?>> StartWalk(Map map, List<Chunk> usableChunks, Vector2Int startPosition)
        {
            Road = new Queue<KeyValuePair<ChunkHolder, CardinalDirections?>>();

            //The first chunk is marked.
            var firstChunk = map.Grid[startPosition.x, startPosition.y];

            //Put the first chunkholder in the road and dont give it a direction.
            Road.Enqueue(new KeyValuePair<ChunkHolder, CardinalDirections?>(firstChunk, null));

            if (!MarkedChunks.Contains(firstChunk))
                MarkedChunks.Enqueue(firstChunk);

            map.StartChunk = firstChunk;

            Vector2Int currentPos = startPosition;

            //We create a list of all the possible directions for the walk, based from the enum.
            ResetDirectionCandidates();

            //We start at a path length of 0
            int pathLength = 0;

            //While we still have more chunks to mark and it hasn't gone stuck yet, keep marking.
            while (pathLength <= _pathLength && DirectionCandidates.Any())
            {
                //Find out what the next chunk could be.
                var possibleSegment = FindNextChunk(map, usableChunks, ref currentPos);

                //If it found a suitable next chunk, put it in the road.
                if (possibleSegment != null)
                {
                    Road.Enqueue(possibleSegment.GetValueOrDefault());
                    pathLength++;
                }
            }

            if (Road.All(pair => pair.Key == firstChunk))
                return null;

            return Road;
        }
    }
}