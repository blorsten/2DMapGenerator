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
    [CreateAssetMenu(fileName = "New Dead End Maker", menuName = "MapGeneration/Algorithms/Dead End Maker")]
    public class DeadEndMaker : DrunkardWalkAlgorithm
    {
        private List<Queue<KeyValuePair<ChunkHolder, CardinalDirections?>>> _roads;

        //Contains all the chunks it should start a drunkard walk from.
        private List<ChunkHolder> _myMarkedChunks;

        //How many entanglements can the dead end maker make.
        [SerializeField] private int _nrOfDeadEnds = 2;

        public override bool Process(Map map, List<Chunk> usableChunks)
        {
            //Reset all collections
            Reset();

            //Find out which chunks has aleready been changed by other algorithms.
            FindMarkedChunks(map);

            //Then start the dead end maker.
            StartWalk(map, usableChunks, Vector2Int.zero);

            if (_roads == null || _roads != null && !_roads.Any())
                return false;

            //Backtacks all roads that has been created.
            _roads.ForEach(BackTrackChunks);

            return true;
        }

        /// <summary>
        /// Find all chunkholders that has been changed by algorithms.
        /// </summary>
        /// <param name="map"></param>
        private void FindMarkedChunks(Map map)
        {
            foreach (ChunkHolder chunk in map.Grid)
            {
                if (!chunk.ChunkOpenings.IsEmpty())
                {
                    MarkedChunks.Enqueue(chunk);
                }
            }
        }

        /// <summary>
        /// Starts the dead ens maker.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="usableChunks"></param>
        /// <param name="startPosition"></param>
        /// <returns></returns>
        protected override Queue<KeyValuePair<ChunkHolder, CardinalDirections?>> StartWalk(Map map, List<Chunk> usableChunks, Vector2Int startPosition)
        {
            //Save references to start and end.
            ChunkHolder start = map.StartChunk;
            ChunkHolder end = map.EndChunk;

            //Create a new queue for the upcomming road.
            _roads = new List<Queue<KeyValuePair<ChunkHolder, CardinalDirections?>>>();

            //Create a copy of the marked chunks, this will be all the chunks we can visit.
            _myMarkedChunks = MarkedChunks.ToList();

            //Iterate until we reached x number of dead ends.
            for (int i = 0; i < _nrOfDeadEnds; i++)
            {
                //If we dont have any marked chunks to start one, break.
                if (!_myMarkedChunks.Any())
                    break;

                ChunkHolder startChunk = _myMarkedChunks[map.Random.Range(0, _myMarkedChunks.Count)];
                startPosition = startChunk.Position;

                //Tries and do a drunkard walk on the marked chunk.
                Queue<KeyValuePair<ChunkHolder, CardinalDirections?>> newRoad = base.StartWalk(map, usableChunks, startPosition);

                //If it succeeded remove it from start candidates and add it to the main road.
                if (newRoad != null)
                {
                    _myMarkedChunks.Remove(startChunk);
                    _roads.Add(newRoad);
                } 
                else 
                {
                    //If it failed to find a new road, dont count it as a dead end iteration.
                    i--;
                }
            }

            //Now that we have made alot of dead ends, set the start and end to the first ones.
            map.StartChunk = start; 
            map.EndChunk = end;
            return null;
        }
    }
}