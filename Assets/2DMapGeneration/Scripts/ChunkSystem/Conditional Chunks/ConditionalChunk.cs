using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration.ChunkSystem
{
    /// <summary>
    /// Purpose: Main component for conditional based chunks.
    /// Creator: PW
    /// </summary>
    [RequireComponent(typeof(Chunk))]
    public class ConditionalChunk : MonoBehaviour 
    {
        [SerializeField] private List<ValidationEntry> _validationStack = new List<ValidationEntry>();

        public bool Validate(Map map, ChunkHolder chunkHolder)
        {
            foreach (var item in _validationStack)
            {
                if (!item.Validate(map, chunkHolder))
                {
                    DisApproved(map, chunkHolder);
                    return false;
                }
            }

            Approved(map,chunkHolder);
            return true;
        }

        public virtual void Approved(Map map, ChunkHolder chunkHolder)
        {
            foreach (var item in _validationStack)
            {
                item.Approved(map,chunkHolder);
            }
        }

        public virtual void DisApproved(Map map, ChunkHolder chunkHolder)
        {
            foreach (var item in _validationStack)
            {
                item.DisApproved(map, chunkHolder);
            }
        }
    }
}