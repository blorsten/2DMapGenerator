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
        [SerializeField] private ChunkOpenings _chunkOpenings;
        [SerializeField] private Vector2Int _position;

        public Vector2Int Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Chunk Prefab
        {
            get { return __prefab; }
            set { __prefab = value; }
        }
        public Chunk Instance { get; set; }

        public ChunkOpenings ChunkOpenings
        {
            get { return _chunkOpenings ?? (_chunkOpenings = new ChunkOpenings()); }
            set { _chunkOpenings = value; }
        }

        public ChunkType ChunkType { get; set; }

        public ChunkHolder(Vector2Int position)
        {
            Position = position;
        }
        /// <summary>
        /// Instantiate Prefab and save in Instance
        /// </summary>
        public Chunk Instantiate(Vector2 position, Map map)
        {
            Instance = Object.Instantiate(Prefab,position,Quaternion.identity);
            Instance.ChunkHolder = this;
            Instance.Map = map;
            return Instance;
        }

        /// <summary>
        /// Instantiate Prefab and save in Instance
        /// </summary>
        public Chunk Instantiate(Vector2 position, Transform parent, Map map)
        {
            Instance = Object.Instantiate(Prefab, position, Quaternion.identity,parent);
            Instance.ChunkHolder = this;
            Instance.Map = map;
            return Instance;
        }
    }
}