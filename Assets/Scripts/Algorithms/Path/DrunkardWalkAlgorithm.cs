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
            //This is where the walk starts.
            Vector2Int startPoint = map.Random.Range(Vector2Int.zero, map.MapBlueprint.GridSize);

            //The first chunk is marked.
            var firstChunk = map.Grid[startPoint.x, startPoint.y];
            MarkedChunks.Enqueue(firstChunk);
            map.StartChunk = firstChunk;
            firstChunk.Prefab = usableChunks.FirstOrDefault();

            Vector2Int currentPos = startPoint;

            //We create a list of all the possible directions for the walk, based from the enum.
            ResetDirectionCandidates();

            //While we still have more chunks to mark and it hasn't gone stuck yet, keep marking.
            while (MarkedChunks.Count <= _pathLength && DirectionCandidates.Any())
                FindNextChunk(map, usableChunks, ref currentPos);
        }

        public override void PostProcess(Map map, List<Chunk> usableChunks)
        {
            BackTrackChunks(MarkedChunks, DirectionsTaken);
            base.PostProcess(map, usableChunks);
        }
    }
}