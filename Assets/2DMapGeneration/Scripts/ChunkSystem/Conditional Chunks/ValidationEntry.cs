using UnityEngine;

namespace MapGeneration.ChunkSystem
{
    /// <summary>
    /// Purpose:
    /// Creator:
    /// </summary>
    public abstract class ValidationEntry : ScriptableObject
    {
        /// <summary>
        /// Called when a conditional chunk is going through all its validation entries and 
        /// validates them.
        /// </summary>
        /// <param name="map">The map that the conditional chunks is in.</param>
        /// <param name="chunkHolder">The chunkholder that the conditional chunk is being
        /// placed in</param>
        /// <returns></returns>
        public abstract bool Validate(Map map, ChunkHolder chunkHolder);

        /// <summary>
        /// Called when everything checks out with the validation entry meaning that the validation
        /// approved.
        /// </summary>
        /// <param name="map">The map that the conditional chunks is in.</param>
        /// <param name="chunkHolder">The chunkholder that the conditional chunk is being
        /// placed in</param>
        public virtual void Approved(Map map, ChunkHolder chunkHolder) { }

        /// <summary>
        /// Called when the validation entry fails its validation.
        /// </summary>
        /// <param name="map">The map that the conditional chunks is in.</param>
        /// <param name="chunkHolder">The chunkholder that the conditional chunk is being
        /// placed in</param>
        public virtual void DisApproved(Map map, ChunkHolder chunkHolder) { }
    }
}