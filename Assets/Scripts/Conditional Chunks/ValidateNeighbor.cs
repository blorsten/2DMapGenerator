using System;
using System.Linq;
using System.Net;
using MapGeneration.Algorithm;
using UnityEngine;

namespace MapGeneration.ConditionalChunks
{
    /// <summary>
    /// Purpose:
    /// Creator:
    /// </summary>
    [CreateAssetMenu(fileName = "Validate Neighbor", menuName = "MapGeneration/Conditional Chunks/ValidateNeighbor")]/// 
    public class ValidateNeighbor : ValidationEntry
    {
        [SerializeField]
        private PathAlgorithm.CardinalDirections _direction;

        [SerializeField, Tooltip("Leave empty if neighbor should be empty")]
        private GameObject __chunkToCheck;

        [SerializeField, Header("On Approved")]
        private Chunk _chunkToSpawn;

        public override bool Validate(Map map, ChunkHolder chunkHolder)
        {
            var neighbors = map.GetNeighbor(chunkHolder, _direction);

            if (!neighbors.Any())
                return false;

            foreach (var item in neighbors)
            {
                if (item.Prefab != null)
                    return false;
            }

            return true;
        }

        public override void Approved(Map map, ChunkHolder chunkHolder)
        {
            base.Approved(map, chunkHolder);

            var newPos = map.GetChunkPos(chunkHolder);

            switch (_direction)
            {
                case PathAlgorithm.CardinalDirections.Top:
                    newPos = new Vector2Int(newPos.x, newPos.y + 1);
                    break;
                case PathAlgorithm.CardinalDirections.Bottom:
                    newPos = new Vector2Int(newPos.x, newPos.y - 1);
                    break;
                case PathAlgorithm.CardinalDirections.Left:
                    newPos = new Vector2Int(newPos.x + 1, newPos.y);
                    break;
                case PathAlgorithm.CardinalDirections.Right:
                    newPos = new Vector2Int(newPos.x -1, newPos.y);
                    break;
            }

            var newHolder = map.Grid[newPos.x, newPos.y];

            switch (_direction)
            {
                case PathAlgorithm.CardinalDirections.Top:
                    newHolder.ChunkOpenings.BottomConnetion = true;
                    chunkHolder.ChunkOpenings.TopConnection = true;
                    break;
                case PathAlgorithm.CardinalDirections.Bottom:
                    newHolder.ChunkOpenings.TopConnection = true;
                    chunkHolder.ChunkOpenings.BottomConnetion = true;
                    break;
                case PathAlgorithm.CardinalDirections.Left:
                    newHolder.ChunkOpenings.RightConnection = true;
                    chunkHolder.ChunkOpenings.LeftConnection = true;
                    break;
                case PathAlgorithm.CardinalDirections.Right:
                    newHolder.ChunkOpenings.LeftConnection = true;
                    chunkHolder.ChunkOpenings.RightConnection = true;
                    break;
            }

            map.Place(newHolder, _chunkToSpawn);
        }
    }
}