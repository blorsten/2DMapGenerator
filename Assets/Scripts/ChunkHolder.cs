using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// Purpose:
    /// Creator:
    /// </summary>
    public struct  ChunkHolder
    {
        public Chunk Prefab { get; set; }
        public Chunk Instance { get; set; }

        /// <summary>
        /// Instantiate Prefab and save in Instance
        /// </summary>
        public void Instantiate()
        {
            Instance = Object.Instantiate(Prefab);
        }
    }
}