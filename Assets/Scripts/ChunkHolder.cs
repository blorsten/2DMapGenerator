using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MapGeneration
{
    /// <summary>
    /// Purpose:
    /// Creator:
    /// </summary>
    [Serializable]
    public class ChunkHolder
    {
        [SerializeField] private Chunk __prefab;
        private ChunkOpenings _chunkOpenings;

        public Chunk Prefab
        {
            get { return __prefab; }
            set { __prefab = value; }
        }
        public Chunk Instance { get; set; }

        public ChunkOpenings ChunkOpenings
        {
            get { if(_chunkOpenings == null)_chunkOpenings = new ChunkOpenings();return _chunkOpenings;}
            set { _chunkOpenings = value; }
        }
        /// <summary>
        /// Instantiate Prefab and save in Instance
        /// </summary>
        public Chunk Instantiate(Vector2 position)
        {
            Instance = Object.Instantiate(Prefab,position,Quaternion.identity);
            Instance.ChunkHolder = this;
            return Instance;
        }

        /// <summary>
        /// Instantiate Prefab and save in Instance
        /// </summary>
        public Chunk Instantiate(Vector2 position, Transform parent)
        {
            Instance = Object.Instantiate(Prefab, position, Quaternion.identity,parent);
            Instance.ChunkHolder = this;
            return Instance;
        }
    }
}