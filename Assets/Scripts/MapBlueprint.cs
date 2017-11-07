using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// Purpose:
    /// Creator: Peter Witt
    /// </summary>
    public class MapBlueprint : ScriptableObject
    {
        [SerializeField]
        private List<MapGenerationAlgorithm> _algorithmStack;

        [SerializeField]
        private Vector2Int _gridSize;

        [SerializeField]
        private Vector2Int _chunkSize;

        [SerializeField]
        private int _userSeed;

        [SerializeField]
        private bool _useWhiteList;

        [SerializeField]
        private List<Chunk> __whitelistedChunks; 

        [SerializeField]
        private bool _useBlackList;

        [SerializeField]
        private List<Chunk> __blacklistedChunks;

        public Vector2Int GridSize { get { return _gridSize; } set { _gridSize = value; } }
        public Vector2Int ChunkSize { get { return _chunkSize; } set { _chunkSize = value; } }
        public int UserSeed { get { return _userSeed; } set { _userSeed = value; } }

        /// <summary>
        /// Generates a map from the blueprint
        /// </summary>
        /// <param name="map">map</param>
        public void Generate(Map map)
        {
            
        }
    }
}