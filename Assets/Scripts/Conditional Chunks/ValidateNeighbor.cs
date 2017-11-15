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
        [SerializeField] private DrunkardWalkAlgorithm.CardinalDirections _direction;
        [SerializeField] private GameObject __chunkToCheck;

        public override bool Validate(Map map, ChunkHolder chunkHolder)
        {
            foreach (var item in map.GetNeighbor(chunkHolder, _direction))
            {
                if (item.Prefab != null)
                    return false;
            }

            return true;
        }
    }
}