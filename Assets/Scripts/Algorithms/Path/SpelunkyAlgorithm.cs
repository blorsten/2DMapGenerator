using System.Collections.Generic;
using UnityEngine;
using MapGeneration.Extensions;
using System.Linq;
using ListExstention;

namespace MapGeneration.Algorithm
{
    /// <summary>
    /// Purpose: 
    /// make a path simular to spenlunkys algorithm, with a critical path, that is always open
    /// Creator: 
    /// Niels Justesen
    /// Mathias Prisfeldt
    /// </summary>
    [CreateAssetMenu(fileName = "Spelunky Algorithm", menuName = "MapGeneration/Algorithms/Spelunky")]
    public class SpelunkyAlgorithm : PathAlgorithm
    {
        public override void Process(Map map, List<Chunk> usableChunks)
        {
            //set the statpoint in the top row of the map grid
            Vector2Int startPoint = new Vector2Int(map.Random.Range(0, map.MapBlueprint.GridSize.x), map.MapBlueprint.GridSize.y - 1);
            
            //The first chunk is marked.
            ChunkHolder firstChunk = map.Grid[startPoint.x, startPoint.y];
            MarkedChunks.Enqueue(firstChunk);
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
            BackTrackChunks(Road);
        }
    }
}