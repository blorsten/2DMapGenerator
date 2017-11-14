using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using MapGeneration.Extensions;

namespace MapGeneration
{
    /// <summary>
    /// Purpose:
    /// make a path for the MapBuilder to fill with chunks
    /// Creator:
    /// Niels Justesen
    /// Mathias Prisfeldt
    /// </summary>

    [CreateAssetMenu(fileName = "Drunkard Walk Algorithm", menuName = "MapGeneration/Algorithms/DrunkardWalk")]
    public class DrunkardWalkAlgorithm : MapGenerationAlgorithm
    {
        //Compass directions used for choosing where to go next.
        enum CardinalDirections
        {
            North, South, East, West
        }

        private CardinalDirections _nextDirection;

        //Chunks that has been visited by the algorithm so it doesn't revisit them. Using a queue, so we can backtrack the elements
        private readonly Queue<ChunkHolder> _markedChunks = new Queue<ChunkHolder>();
        private Queue<CardinalDirections> _dirrections = new Queue<CardinalDirections>();

        //Number of times the algorithms creates a marked chunk.
        [SerializeField] private int _pathLength;

        public override void Process(Map map, List<Chunk> usableChunks)
        {
            //First we reset the algorithm
            _markedChunks.Clear();

            //This is where the walk starts.
            Vector2Int startPoint = map.Random.Range(Vector2Int.zero, map.MapBlueprint.GridSize);

            //The first chunk is marked.
            var firstChunk = map.Grid[startPoint.x, startPoint.y];
            _markedChunks.Enqueue(firstChunk);
            map.StartChunk = firstChunk;

            Vector2Int currentPos = startPoint;

            //We create a list of all the possible directions for the walk, based from the enum.
            List<CardinalDirections> candidates = ((CardinalDirections[])Enum.GetValues(typeof(CardinalDirections))).ToList();

            //While we still have more chunks to mark and it hasn't gone stuck yet, keep marking.
            while (_markedChunks.Count <= _pathLength && candidates.Any())
            {
                //Find out what are next direction would be.
                _nextDirection = candidates[map.Random.Next(0, candidates.Count)];

                //Find out the next position and check if its valid.
                Vector2Int? nextPosition = CheckNextPosition(currentPos, _nextDirection, map);
                if (nextPosition != null)
                {
                    //If the next position is valid grab the chunk on that position.
                    ChunkHolder nextChunk = map.Grid[nextPosition.Value.x, nextPosition.Value.y];

                    //If it hasn't been marked yet, mark it.
                    if (!_markedChunks.Contains(nextChunk))
                    {
                        _dirrections.Enqueue(_nextDirection);

                        currentPos = nextPosition.Value;
                        _markedChunks.Enqueue(nextChunk);
                        //Change the prefab on the found chunk to another one. TODO: Find another way to mark marked chunks.
                        nextChunk.Prefab = usableChunks.FirstOrDefault();

                        //Reset candidates.
                        candidates = ((CardinalDirections[])Enum.GetValues(typeof(CardinalDirections))).ToList();
                    }
                    else
                        candidates.Remove(_nextDirection);
                }
                else
                    candidates.Remove(_nextDirection);
            }

            map.EndChunk = _markedChunks.LastOrDefault();
        }

        /// <summary>
        /// Takes a current position and checks out from a directions if its a valid move on the grid.
        /// </summary>
        /// <param name="currentPosition"></param>
        /// <param name="nextDir"></param>
        /// <param name="currentMap"></param>
        /// <returns></returns>
        private Vector2Int? CheckNextPosition(Vector2Int currentPosition, CardinalDirections nextDir, Map currentMap)
        {
            switch (nextDir)
            {
                case CardinalDirections.North:
                    currentPosition += new Vector2Int(0, 1);
                    if (currentPosition.y < currentMap.MapBlueprint.GridSize.y && currentPosition.y >= 0)
                    {
                        return currentPosition;
                    }
                    break;
                case CardinalDirections.South:
                    currentPosition += new Vector2Int(0, -1);
                    if (currentPosition.y < currentMap.MapBlueprint.GridSize.y && currentPosition.y >= 0)
                    {
                        return currentPosition;
                    }
                    break;
                case CardinalDirections.East:
                    currentPosition += new Vector2Int(1, 0);
                    if (currentPosition.x < currentMap.MapBlueprint.GridSize.x && currentPosition.x >= 0)
                    {
                        return currentPosition;
                    }
                    break;
                case CardinalDirections.West:
                    currentPosition += new Vector2Int(-1, 0);
                    if (currentPosition.x < currentMap.MapBlueprint.GridSize.x && currentPosition.x >= 0)
                    {
                        return currentPosition;
                    }
                    break;
            }

            return null;
        }

        public override void PostProcess(Map map, List<Chunk> usableChunks)
        {
            Color startColor = new Color(1, 0, 0);

            float incrementer = 1f / _markedChunks.Count;

            foreach (ChunkHolder markedChunk in _markedChunks)
            {
                if (markedChunk.Instance)
                    markedChunk.Instance.GetComponent<SpriteRenderer>().color = startColor += new Color(0, incrementer, incrementer);
            }
            BacktrakAlgorithm(_markedChunks, _dirrections);

            base.PostProcess(map, usableChunks);
        }

        /// <summary>
        /// Backtracks all the chunks in the queue
        /// </summary>
        /// <param name="chunks">Chunks in the queue</param>
        /// <param name="direction">Dirrections taken</param>
        private void BacktrakAlgorithm(Queue<ChunkHolder> chunks, Queue<CardinalDirections> direction)
        {
            CardinalDirections currentDirection = CardinalDirections.North;
            while (chunks.Count > 0)
            {
                ChunkHolder currentChunk = chunks.Dequeue();

                if (direction.Count > 0)
                    currentDirection = direction.Dequeue();

                if (currentChunk.Instance && chunks.Count > 0)
                    SetChunkConnections(currentDirection, currentChunk, chunks.First());
            }
        }

        /// <summary>
        /// Sets the connections in the queue of marked chunks
        /// </summary>
        /// <param name="dirr">The direction the algorithm took</param>
        /// <param name="current">the current chunk</param>
        /// <param name="next">the next chunk in the queue</param>
        private void SetChunkConnections(CardinalDirections dirr, ChunkHolder current, ChunkHolder next)
        {
            switch (dirr)
            {
                case CardinalDirections.North:
                    current.Instance.ChunkHolder.ChunkOpenings.TopConnection = true;
                    next.Instance.ChunkHolder.ChunkOpenings.BottomConnetion = true;
                    break;
                case CardinalDirections.South:
                    current.Instance.ChunkHolder.ChunkOpenings.BottomConnetion = true;
                    next.Instance.ChunkHolder.ChunkOpenings.TopConnection = true;
                    break;
                case CardinalDirections.East:
                    current.Instance.ChunkHolder.ChunkOpenings.RightConnection = true;
                    next.Instance.ChunkHolder.ChunkOpenings.LeftConnection = true;
                    break;
                case CardinalDirections.West:
                    current.Instance.ChunkHolder.ChunkOpenings.LeftConnection = true;
                    next.Instance.ChunkHolder.ChunkOpenings.RightConnection = true;
                    break;
            }
        }
    }
}