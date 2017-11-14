using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// Purpose:
    /// Creator:
    /// </summary>
    [ExecuteInEditMode]
    public class ResourceHandler : Singleton<ResourceHandler>
    {
        [SerializeField]
        private List<Chunk> _chunks;

        public List<Chunk> Chunks { get { return _chunks; } private set { _chunks = value; } }

        protected override void Awake()
        {
            base.Awake();
            UpdateResources();
        }

        /// <summary>
        /// Loads all chunks from resources
        /// </summary>
        [ContextMenu("Update Chunks")]
        public void UpdateResources()
        {
            Chunks = new List<Chunk>();
            Chunks.AddRange(Resources.LoadAll<Chunk>(string.Empty));
        }
    }
}