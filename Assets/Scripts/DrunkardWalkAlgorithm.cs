using System.Collections.Generic;
using UnityEngine;
using Extensions;
using System;

namespace MapGeneration
{
    /// <summary>
    /// Purpose:
    /// make a path for the MapBuilder to fill with chunks
    /// Creator:
    /// Niels Justesen
    /// </summary>



    [CreateAssetMenu(fileName = "Drunkard Walk Algorithm", menuName = "MapGeneration/Algorithms/DrunkardWalk")]
    public class DrunkardWalkAlgorithm : MapGenerationAlgorithm
    {
        [Flags]
        enum CardinalDirections
        {
            North, South, East, West
        }

        private CardinalDirections _direction;

        private List<ChunkHolder> _markedChunks = new List<ChunkHolder>();

        [SerializeField] private int _iterations;

        public override void Process(Map map, List<Chunk> usableChunks)
        {
            Vector2Int startPoint = map.Random.Range(Vector2Int.zero, map.MapBlueprint.GridSize);
            _markedChunks.Add(map.Grid[startPoint.x, startPoint.y]);
            Vector2Int currentPos = startPoint;
            CardinalDirections[] candidates = (CardinalDirections[]) Enum.GetValues(typeof(CardinalDirections));

            while (_markedChunks.Count <= _iterations)
            {
                _direction = (CardinalDirections)map.Random.Range(0, 4);
                

                Vector2Int? nextPosition = CheckNextPosition(currentPos, _direction, map);
                if (nextPosition != null)
                {
                    ChunkHolder nextChunk = map.Grid[nextPosition.Value.x, nextPosition.Value.y];
                    if (!_markedChunks.Contains(nextChunk))
                    {
                        _markedChunks.Add(nextChunk);
                    }
                }
            }
        }

        private Vector2Int? CheckNextPosition(Vector2Int nextPosition, CardinalDirections nextDir, Map currentMap)
        {
            Vector2Int? currentPosition = null;
            switch (nextDir)
            {
                case CardinalDirections.North:
                    currentPosition = nextPosition + new Vector2Int(0, 1);
                    if (currentPosition.Value.y < currentMap.MapBlueprint.GridSize.y)
                    {
                        return currentPosition;
                    }
                    break;
                case CardinalDirections.South:
                    currentPosition = nextPosition + new Vector2Int(0, -1);
                    if (currentPosition.Value.y < currentMap.MapBlueprint.GridSize.y)
                    {
                        return currentPosition;
                    }
                    break;
                case CardinalDirections.East:
                    currentPosition = nextPosition + new Vector2Int(1, 0);
                    if (currentPosition.Value.x < currentMap.MapBlueprint.GridSize.x)
                    {
                        return currentPosition;
                    }
                    break;
                case CardinalDirections.West:
                    currentPosition = nextPosition + new Vector2Int(-1, 0);
                    if (currentPosition.Value.x < currentMap.MapBlueprint.GridSize.x)
                    {
                        return currentPosition;
                    }
                    break;
            }
            return currentPosition;
        }
    }
}