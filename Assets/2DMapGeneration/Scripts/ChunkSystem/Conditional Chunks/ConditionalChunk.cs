using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration.ChunkSystem
{
    /// <summary>
    /// Main component for conditional based chunks.
    /// </summary>
    [RequireComponent(typeof(Chunk))]
    public class ConditionalChunk : MonoBehaviour
    {
        [SerializeField] private List<ValidationEntry> _validationStack = new List<ValidationEntry>();

        /// <summary>
        /// Validate the chunk, based on the validationsEntry objects in the _validationStack list
        /// </summary>
        /// <param name="map">The current map</param>
        /// <param name="chunkHolder">Chunkholder to place in</param>
        /// <returns>Weather or not the chunk could be validated</returns>
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

            Approved(map, chunkHolder);
            return true;
        }

        /// <summary>
        /// This is run if the chunk could be validated. It runs all validationEntry onApproved methods
        /// </summary>
        /// <param name="map">Current map</param>
        /// <param name="chunkHolder">Chunkholder to place in</param>
        public virtual void Approved(Map map, ChunkHolder chunkHolder)
        {
            foreach (var item in _validationStack)
            {
                item.Approved(map, chunkHolder);
            }
        }

        /// <summary>
        /// This is run if the chunk could not be validated. It runs all validationEntry
        /// OnDisApproved methods
        /// </summary>
        /// <param name="map"></param>
        /// <param name="chunkHolder"></param>
        public virtual void DisApproved(Map map, ChunkHolder chunkHolder)
        {
            foreach (var item in _validationStack)
            {
                item.DisApproved(map, chunkHolder);
            }
        }
    }
}