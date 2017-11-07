using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// Purpose:
    /// Creator:
    /// </summary>
    public class ResourceHandler : Singleton<ResourceHandler>
    {
        [SerializeField]
        private List<Chunk> _chunks;

        public List<Chunk> Chunks { get { return _chunks; } set { _chunks = value; } }

        /// <summary>
        /// Loads all chunks from resources
        /// </summary>
        public void Load()
        {
            
        }
    }
}