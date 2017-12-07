using System;
using System.Collections.Generic;
using System.Linq;
using MapGeneration.ChunkSystem;
using MapGeneration.Extensions;
using UnityEngine;

namespace MapGeneration.Algorithm
{
    /// <summary>
    /// Purpose: Base class for all path algortihms.
    /// Creator: MP & NJ
    /// </summary>
    public class PathAlgorithm : MapGenerationAlgorithm
    {
        //Compass directions used for choosing where to go next.
        [Flags]
        public enum CardinalDirections
        {
            Top,
            Bottom,
            Left,
            Right
        }

        protected CardinalDirections NextDirection;
        protected Queue<ChunkHolder> MarkedChunks = new Queue<ChunkHolder>();

        //Collection used to store the road the path algorithm took.
        protected Queue<KeyValuePair<ChunkHolder, CardinalDirections?>> Road = new Queue<KeyValuePair<ChunkHolder, CardinalDirections?>>();

        //Collection used to store all the directions the path algorithm can take.
        protected List<CardinalDirections> DirectionCandidates;

        public override bool Process(Map map, List<Chunk> usableChunks)
        {
            bool success = base.Process(map, usableChunks);
            Reset();
            return success;
        }

        /// <summary>
        /// Backtraks both queues
        /// </summary>
        /// <param name="chunks">Chunks to backtrack</param>
        /// <param name="directions">Directions to backtrack</param>
        /// <param name="type">The desired connection type for for backtracking.</param>
        protected void BackTrackChunks(Queue<ChunkHolder> chunks, Queue<CardinalDirections> directions, ChunkOpenings.ConnectionType type = ChunkOpenings.ConnectionType.Default)
        {
            CardinalDirections currentDirection = CardinalDirections.Bottom;

            //While there is still chunks to make connections from do so.
            while (chunks.Count > 0)
            {
                ChunkHolder currentChunk = chunks.Dequeue(); //Grab a chunkholder from queue

                //If there still are directions, take one.
                if (directions.Count > 0)
                    currentDirection = directions.Dequeue();

                //Set a direction from current to previous.
                if (currentChunk.Instance && chunks.Count > 0)
                    SetChunkConnections(currentDirection, currentChunk, chunks.First(), type);
            }
        }

        /// <summary>
        /// Backtraks both queues
        /// </summary>
        /// <param name="road"></param>
        /// <param name="type">The desired connection type for for backtracking.</param>
        protected void BackTrackChunks(Queue<KeyValuePair<ChunkHolder, CardinalDirections?>> road, ChunkOpenings.ConnectionType type = ChunkOpenings.ConnectionType.Default)
        {
            //Dont make any connections if the road are not long enough.
            if (road.Count < 2)
                return;

            CardinalDirections currentDirection = CardinalDirections.Bottom;

            //Grab the first chunkholder, which doesnt have a from direction.
            ChunkHolder current = road.Dequeue().Key;

            while (road.Count > 0)
            {
                KeyValuePair<ChunkHolder, CardinalDirections?> currentChunk = road.Dequeue();

                if (currentChunk.Value != null)
                    currentDirection = currentChunk.Value.Value;

                SetChunkConnections(currentDirection, current, currentChunk.Key, type);

                current = currentChunk.Key;
            }
        }

        /// <summary>
        /// Resets direction candidates back to a list full of directions.
        /// </summary>
        protected void ResetDirectionCandidates()
        {
            DirectionCandidates = ((CardinalDirections[])Enum.GetValues(typeof(CardinalDirections))).ToList();
        }

        /// <summary>
        /// Tries to find a suitable chunk from a position.
        /// </summary>
        /// <param name="map">Map it works on.</param>
        /// <param name="usableChunks">All usable chunks.</param>
        /// <param name="currentPos"></param>
        /// <returns>Returns the chunkholder it found and what direction it took. Is null if it dident find a new chunkholder.</returns>
        protected KeyValuePair<ChunkHolder, CardinalDirections?>? FindNextChunk(Map map, List<Chunk> usableChunks, ref Vector2Int currentPos)
        {
            //find the next direction among the candidates.
            NextDirection = DirectionCandidates[map.Random.Range(0, DirectionCandidates.Count)];

            //Get the next position
            Vector2Int? nextPosition = CheckNextPosition(currentPos, NextDirection, map);

            //if the position is valid continue the process
            if (nextPosition != null)
            {
                ChunkHolder nextChunk = map.Grid[nextPosition.Value.x, nextPosition.Value.y];
                ChunkHolder currChunkHolder = map.Grid[currentPos.x, currentPos.y];

                bool isChunkValid = !(currChunkHolder.Prefab && !currChunkHolder.ChunkOpenings.IsOpen(NextDirection));

                //if the next chunk isnt marked, continue the process
                if (!MarkedChunks.Contains(nextChunk) && isChunkValid)
                {
                    //set current position to the next position
                    currentPos = nextPosition.Value;

                    //enqueue the next chunk, so we know it is used.
                    MarkedChunks.Enqueue(nextChunk);

                    //Reset candidates.
                    ResetDirectionCandidates();

                    map.EndChunk = MarkedChunks.LastOrDefault();
                    return new KeyValuePair<ChunkHolder, CardinalDirections?>(nextChunk, NextDirection);
                }

                DirectionCandidates.Remove(NextDirection);
            }
            else
                DirectionCandidates.Remove(NextDirection);

            return null;
        }

        protected override void Reset()
        {
            base.Reset();
            MarkedChunks.Clear();
            ResetDirectionCandidates();
            Road.Clear();
        }
    }
}