using System.Collections.Generic;
using UnityEngine;
using MapGeneration.Extensions;
using System.Linq;

namespace MapGeneration.Algorithm
{
    /// <summary>
    /// Purpose:
    /// Make dead end path out from the first path made by and algorithm
    /// Creator:
    /// NJ og MP
    /// </summary>
    [CreateAssetMenu(fileName = "Dead End Algorithm", menuName = "MapGeneration/Algorithms/DeadEndMaker")]
    public class DeadEndMaker : DrunkardWalkAlgorithm
    {
        private List<Queue<KeyValuePair<ChunkHolder, CardinalDirections?>>> _roads;
        private List<ChunkHolder> _myMarkedChunks;

        [SerializeField] private int _nrOfDeadEnds;

        public override void Process(Map map, List<Chunk> usableChunks)
        {
            Reset();
            FindMarkedChunks(map);
            StartWalk(map, usableChunks, Vector2Int.zero);
        }

        private void FindMarkedChunks(Map map)
        {
            foreach (ChunkHolder chunk in map.Grid)
            {
                if (chunk.Prefab != null)
                {
                    MarkedChunks.Enqueue(chunk);
                }
            }
        }

        public override void PostProcess(Map map, List<Chunk> usableChunks)
        {
            _roads.ForEach(BackTrackChunks);
        }

        protected override Queue<KeyValuePair<ChunkHolder, CardinalDirections?>> StartWalk(Map map, List<Chunk> usableChunks, Vector2Int startPosition)
        {
            ChunkHolder start = map.StartChunk;
            ChunkHolder end = map.EndChunk;

            _roads = new List<Queue<KeyValuePair<ChunkHolder, CardinalDirections?>>>();
            _myMarkedChunks = MarkedChunks.ToList();
            for (int i = 0; i < _nrOfDeadEnds; i++)
            {
                if (!_myMarkedChunks.Any())
                    return null;

                ChunkHolder startChunk = _myMarkedChunks[map.Random.Range(0, _myMarkedChunks.Count)];
                startPosition = startChunk.Position;

                Queue<KeyValuePair<ChunkHolder, CardinalDirections?>> newRoad = base.StartWalk(map, usableChunks, startPosition);
                if (newRoad != null)
                {
                    _myMarkedChunks.Remove(startChunk);
                    _roads.Add(newRoad);
                } 
                else 
                {
                    i--;
                }
            }

            map.StartChunk = start; 
            map.EndChunk = end;
            return null;
        }
    }
}