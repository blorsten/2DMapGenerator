using UnityEngine;

namespace MapGeneration.ConditionalChunks
{
    /// <summary>
    /// Purpose:
    /// Creator:
    /// </summary>
    public abstract class ValidationEntry : ScriptableObject
    {
        public abstract bool Validate(Map map, ChunkHolder chunkHolder);
        public virtual void Approved(Map map, ChunkHolder chunkHolder) { }
        public virtual void DisApproved(Map map, ChunkHolder chunkHolder) { }
    }
}