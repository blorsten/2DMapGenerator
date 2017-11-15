using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using MapGeneration.Extensions;

namespace MapGeneration.Algorithm
{
    /// <summary>
    /// Purpose:
    /// make a path for the MapBuilder to fill with chunks
    /// Creator:
    /// Niels Justesen
    /// Mathias Prisfeldt
    /// </summary>
    [CreateAssetMenu(fileName = "Drunkard Walk Algorithm", menuName = "MapGeneration/Algorithms/DrunkardWalk")]
    public class DrunkardWalkAlgorithm : PathAlgorithm
    {

        //Number of times the algorithms creates a marked chunk.
        [SerializeField] private int _pathLength;

        public override void Process(Map map, List<Chunk> usableChunks)
        {
            base.Process(map, usableChunks);

            //This is where the walk starts.
            Vector2Int startPoint = map.Random.Range(Vector2Int.zero, map.MapBlueprint.GridSize);

            //The first chunk is marked.
            StartWalk(map, usableChunks, startPoint);
        }

        public override void PostProcess(Map map, List<Chunk> usableChunks)
        {
            BackTrackChunks(Road);
            base.PostProcess(map, usableChunks);
        }

        protected virtual Queue<KeyValuePair<ChunkHolder, CardinalDirections?>> StartWalk(Map map, List<Chunk> usableChunks, Vector2Int startPosition)
        {
            Road = new Queue<KeyValuePair<ChunkHolder, CardinalDirections?>>();

            //The first chunk is marked.
            var firstChunk = map.Grid[startPosition.x, startPosition.y];

            Road.Enqueue(new KeyValuePair<ChunkHolder, CardinalDirections?>(firstChunk, null));

            if (!MarkedChunks.Contains(firstChunk))
                MarkedChunks.Enqueue(firstChunk);

            map.StartChunk = firstChunk;
            firstChunk.Prefab = usableChunks.FirstOrDefault();

            Vector2Int currentPos = startPosition;

            //We create a list of all the possible directions for the walk, based from the enum.
            ResetDirectionCandidates();

            int pathLength = 0;

            //While we still have more chunks to mark and it hasn't gone stuck yet, keep marking.
            while (pathLength <= _pathLength && DirectionCandidates.Any())
            {
                var possibleSegment = FindNextChunk(map, usableChunks, ref currentPos);

                if (possibleSegment != null)
                {
                    Road.Enqueue(possibleSegment.GetValueOrDefault());
                    pathLength++;
                }
            }

            return Road;
        }
    }
}