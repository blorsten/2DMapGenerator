using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MapGeneration.ChunkSystem
{
    /// <summary>
    /// Holds data about a positon in the <see cref="Map.Grid"/>
    /// before and after spawning.
    /// </summary>
    [Serializable]
    public class ChunkHolder
    {
        //The prefab recipe on what to instantiate when its spawned.
        [SerializeField] private Chunk __prefab;

        [SerializeField] private ChunkOpenings _chunkOpenings;
        [SerializeField] private Vector2Int _position;

        //Reference to the instantiated chunk when spawned.
        [SerializeField, HideInInspector] private Chunk _instance;
        [SerializeField, HideInInspector] private ChunkType _chunkType;

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

        public Chunk Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }

        public ChunkOpenings ChunkOpenings
        {
            get { return _chunkOpenings ?? (_chunkOpenings = new ChunkOpenings()); }
            set { _chunkOpenings = value; }
        }

        public ChunkType ChunkType
        {
            get { return _chunkType; }
            set { _chunkType = value; }
        }

        public ChunkHolder(Vector2Int position)
        {
            Position = position;
        }
        /// <summary>
        /// Instantiate Prefab and save in Instance
        /// </summary>
        public Chunk Instantiate(Vector2 position, Map map)
        {
            Instance = Object.Instantiate(Prefab, position, Quaternion.identity);
            Instance.RecipeReference = Prefab;
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
            Instance.RecipeReference = Prefab;
            Instance.ChunkHolder = this;
            Instance.Map = map;
            return Instance;
        }
    }
}