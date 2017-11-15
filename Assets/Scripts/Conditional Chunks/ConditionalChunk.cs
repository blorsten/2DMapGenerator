using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration.ConditionalChunks
{
    /// <summary>
    /// Purpose:
    /// Creator:
    /// </summary>
    public class ConditionalChunk : Chunk 
    {
        [SerializeField, Header("Conditional Chunk")]
        private List<ValidationEntry> _validationStack = new List<ValidationEntry>();

        public bool Validate(Map map, ChunkHolder chunkHolder)
        {
            foreach (var item in _validationStack)
            {
                if (!item.Validate(map, chunkHolder))
                {
                    DisApproved(map);
                    return false;
                }
            }

            Approved(map);
            return true;
        }

        public virtual void Approved(Map map) { }
        public virtual void DisApproved(Map map) { }
    }
}