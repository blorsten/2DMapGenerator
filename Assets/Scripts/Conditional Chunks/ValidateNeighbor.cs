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
        [SerializeField] private PathAlgorithm.CardinalDirections _direction;
        [SerializeField, Tooltip("Leave empty if neighbor should be empty")]
        private GameObject __chunkToCheck;

        public override bool Validate(Map map, ChunkHolder chunkHolder)
        {
            foreach (var item in map.GetNeighbor(chunkHolder, _direction))
            {
                if (item.Prefab != __chunkToCheck)
                    return false;
            }

            return true;
        }
    }
}