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
        [SerializeField] private int _nrOfDeadEnds;

        private List<ChunkHolder> _usedChunks = new List<ChunkHolder>();

        private List<ChunkHolder> _myMarkedChunks;

        public override void Process(Map map, List<Chunk> usableChunks)
        {
            _usedChunks.Clear();

            FindMarkedChunks(map);
            base.Process(map, usableChunks);
        }

        public override void PostProcess(Map map, List<Chunk> usableChunks)
        {
            base.PostProcess(map, usableChunks);
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

        protected override bool StartWalk(Map map, List<Chunk> usableChunks, Vector2Int startPosition)
        {
            ChunkHolder start = map.StartChunk;
            ChunkHolder end = map.EndChunk;

            _myMarkedChunks = MarkedChunks.ToList();
            for (int i = 0; i < _nrOfDeadEnds; i++)
            {
                if (_usedChunks.Count >= MarkedChunks.Count || !_myMarkedChunks.Any())
                {
                    return true;
                }

                ChunkHolder startChunk = _myMarkedChunks[map.Random.Range(0, _myMarkedChunks.Count)];
                startPosition = startChunk.Position;

                if (!_usedChunks.Contains(startChunk))
                {
                    _usedChunks.Add(startChunk);
                }
                else
                {
                    i--;
                }


                if (base.StartWalk(map, usableChunks, startPosition))
                {
                    _myMarkedChunks.Remove(startChunk);
                }
                else
                {
                    i--;
                }

            }

            map.StartChunk = start;
            map.EndChunk = end;
            return true;
        }
    }
}