using System.Collections.Generic;
using UnityEngine;
using MapGeneration.Extensions;
using System;
using System.Linq;

namespace MapGeneration
{
    /// <summary>
    /// Purpose: 
    /// make a path simular to spenlunkys algorithm, with a critical path, that is always open
    /// Creator: 
    /// Niels Justesen
    /// </summary>
    [CreateAssetMenu(fileName = "Spelunky Algorithm", menuName = "MapGeneration/Algorithms/Spelunky")]
    public class SpelunkyAlgorithm : MapGenerationAlgorithm
    {
        enum Direction
        {
            Down,
            Left,
            Right
        }

        private Direction _nextDirection;
        private Queue<ChunkHolder> _markedChunks = new Queue<ChunkHolder>();
        private Queue<Direction> _directionsTaken = new Queue<Direction>();
        private List<ChunkHolder> _unsetChunks = new List<ChunkHolder>();

        public override void Process(Map map, List<Chunk> usableChunks)
        {
            //Clear all collections, before starting on a new path.
            _markedChunks.Clear();
            _directionsTaken.Clear();
            _unsetChunks.Clear();
            //set the statpoint in the top row of the map grid
            Vector2Int startPoint = new Vector2Int(map.Random.Range(0, map.MapBlueprint.GridSize.x), map.MapBlueprint.GridSize.y - 1);
            //The first chunk is marked.
            ChunkHolder firstChunk = map.Grid[startPoint.x, startPoint.y];
            _markedChunks.Enqueue(firstChunk);
            map.StartChunk = firstChunk;
            firstChunk.Prefab = usableChunks.FirstOrDefault();

            Vector2Int currentPos = startPoint;

            //Establish candidates for directions to take
            List<Direction> candidates = ((Direction[])Enum.GetValues(typeof(Direction))).ToList();

            //If there are no more candidates, the work is done.
            while (candidates.Count > 0)
            {
                //find the next direction among the candidates.
                _nextDirection = candidates[map.Random.Range(0, candidates.Count)];

                //Get the next position
                Vector2Int? nextPosition = CheckNextPosition(currentPos, _nextDirection, map);

                //if the position is valid continue the process
                if (nextPosition != null)
                {

                    ChunkHolder nextChunk = map.Grid[nextPosition.Value.x, nextPosition.Value.y];

                    //if the next chunk isnt marked, continue the process
                    if (!_markedChunks.Contains(nextChunk))
                    {
                        //enqueue the direction that was taken
                        _directionsTaken.Enqueue(_nextDirection);

                        //set current position to the next position
                        currentPos = nextPosition.Value;

                        //enqueue the next chunk, so we know it is used.
                        _markedChunks.Enqueue(nextChunk);

                        //Change the prefab on the found chunk to another one. TODO: Find another way to mark marked chunks.
                        nextChunk.Prefab = usableChunks.FirstOrDefault();
                        //Reset candidates.
                        candidates = ((Direction[])Enum.GetValues(typeof(Direction))).ToList();
                    }
                    else
                        candidates.Remove(_nextDirection);
                }
                else
                    candidates.Remove(_nextDirection);
            }

            map.EndChunk = _markedChunks.LastOrDefault();
            FillOutUnsetChunks(map, usableChunks);
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

            BackTrackChunks(_markedChunks, _directionsTaken);
            base.PostProcess(map, usableChunks);
        }

        /// <summary>
        /// Check if the next position is valid
        /// </summary>
        /// <param name="currentPos">Current position in the map grid</param>
        /// <param name="nextDir">The next direction to take</param>
        /// <param name="currentMap">The map to use</param>
        /// <returns></returns>
        private Vector2Int? CheckNextPosition(Vector2Int currentPos, Direction nextDir, Map currentMap)
        {
            switch (nextDir)
            {
                case Direction.Down:
                    currentPos += new Vector2Int(0, -1);
                    if (currentPos.y >= 0)
                    {
                        return currentPos;
                    }
                    break;
                case Direction.Left:
                    currentPos += new Vector2Int(-1, 0);
                    if (currentPos.x >= 0)
                    {
                        return currentPos;
                    }
                    break;
                case Direction.Right:
                    currentPos += new Vector2Int(1, 0);
                    if (currentPos.x < currentMap.MapBlueprint.GridSize.x)
                    {
                        return currentPos;
                    }
                    break;
                default:
                    break;
            }
            return null;
        }

        /// <summary>
        /// Backtraks both queues
        /// </summary>
        /// <param name="chunks">Chunks to backtrack</param>
        /// <param name="directions">Directions to backtrack</param>
        private void BackTrackChunks(Queue<ChunkHolder> chunks, Queue<Direction> directions)
        {
            Direction currentDirection = Direction.Down;
            while (chunks.Count > 0)
            {
                ChunkHolder currentChunk = chunks.Dequeue();

                if (directions.Count > 0)
                    currentDirection = directions.Dequeue();

                if (currentChunk.Instance && chunks.Count > 0)
                    SetChunkConnections(currentDirection, currentChunk, chunks.First());
            }
        }

        /// <summary>
        /// Setting the apropriate connections between chunks
        /// </summary>
        /// <param name="currDirection">The direction taken at the current chunk</param>
        /// <param name="current">Current chunk</param>
        /// <param name="next">Next chunk in the queue</param>
        private void SetChunkConnections(Direction currDirection, ChunkHolder current, ChunkHolder next)
        {
            switch (currDirection)
            {
                case Direction.Down:
                    current.Instance.ChunkHolder.ChunkOpenings.BottomConnetion = true;
                    next.Instance.ChunkHolder.ChunkOpenings.TopConnection = true;
                    break;
                case Direction.Left:
                    current.Instance.ChunkHolder.ChunkOpenings.LeftConnection = true;
                    next.Instance.ChunkHolder.ChunkOpenings.RightConnection = true;
                    break;
                case Direction.Right:
                    current.Instance.ChunkHolder.ChunkOpenings.RightConnection = true;
                    next.Instance.ChunkHolder.ChunkOpenings.LeftConnection = true;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Fill out the rest of the map
        /// </summary>
        /// <param name="map"></param>
        /// <param name="usableChunks"></param>
        private void FillOutUnsetChunks(Map map, List<Chunk> usableChunks)
        {
            //Find all unused chunks in the map
            foreach (ChunkHolder chunkHolder in map.Grid)
            {
                if (!chunkHolder.Prefab)
                {
                    _unsetChunks.Add(chunkHolder);
                }
            }

            //Set the prefab of all unused chunks
            foreach (ChunkHolder chunkHolder in _unsetChunks)
            {
                chunkHolder.Prefab = usableChunks[1];
            }
        }
    }
}