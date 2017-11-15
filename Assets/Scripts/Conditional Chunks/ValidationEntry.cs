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
    }
}