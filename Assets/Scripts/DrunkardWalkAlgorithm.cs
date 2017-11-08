using System.Collections.Generic;
using UnityEngine;
using Extensions;
using System;
using System.Linq;

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
        enum CardinalDirections
        {
            North, South, East, West
        }

        private CardinalDirections _currentDirection;

        private readonly List<ChunkHolder> _markedChunks = new List<ChunkHolder>();

        [SerializeField] private int _iterations;

        public override void Process(Map map, List<Chunk> usableChunks)
        {
            Vector2Int startPoint = map.Random.Range(Vector2Int.zero, map.MapBlueprint.GridSize);
            _markedChunks.Add(map.Grid[startPoint.x, startPoint.y]);
            Vector2Int currentPos = startPoint;

            List<CardinalDirections> candidates = ((CardinalDirections[]) Enum.GetValues(typeof(CardinalDirections))).ToList();

            while (_markedChunks.Count <= _iterations && candidates.Any())
            {
                _currentDirection = candidates[map.Random.Next(0, candidates.Count)];

                Vector2Int? nextPosition = CheckNextPosition(currentPos, _currentDirection, map);
                if (nextPosition != null)
                {
                    ChunkHolder nextChunk = map.Grid[nextPosition.Value.x, nextPosition.Value.y];
                    if (!_markedChunks.Contains(nextChunk))
                    {
                        currentPos = nextPosition.Value;
                        _markedChunks.Add(nextChunk);
                        nextChunk.Prefab = usableChunks.FirstOrDefault();
                        candidates = ((CardinalDirections[])Enum.GetValues(typeof(CardinalDirections))).ToList();
                    }
                }
                else
                {
                    candidates.Remove(_currentDirection);
                }
            }

            map.StartChunk = _markedChunks.FirstOrDefault();
            map.EndChunk = _markedChunks.LastOrDefault();
        }

        private Vector2Int? CheckNextPosition(Vector2Int nextPosition, CardinalDirections nextDir, Map currentMap)
        {
            Vector2Int? currentPosition;

            switch (nextDir)
            {
                case CardinalDirections.North:
                    currentPosition = nextPosition + new Vector2Int(0, 1);
                    if (currentPosition.Value.y < currentMap.MapBlueprint.GridSize.y && currentPosition.Value.y >= 0)
                    {
                        return currentPosition;
                    }
                    break;
                case CardinalDirections.South:
                    currentPosition = nextPosition + new Vector2Int(0, -1);
                    if (currentPosition.Value.y < currentMap.MapBlueprint.GridSize.y && currentPosition.Value.y >= 0)
                    {
                        return currentPosition;
                    }
                    break;
                case CardinalDirections.East:
                    currentPosition = nextPosition + new Vector2Int(1, 0);
                    if (currentPosition.Value.x < currentMap.MapBlueprint.GridSize.x && currentPosition.Value.x >= 0)
                    {
                        return currentPosition;
                    }
                    break;
                case CardinalDirections.West:
                    currentPosition = nextPosition + new Vector2Int(-1, 0);
                    if (currentPosition.Value.x < currentMap.MapBlueprint.GridSize.x && currentPosition.Value.x >= 0)
                    {
                        return currentPosition;
                    }
                    break;
            }

            return null;
        }
    }
}