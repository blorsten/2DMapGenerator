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
        [SerializeField] private List<MapGenerationAlgorithm> _algorithmStack;
        [SerializeField] private Vector2Int _gridSize;
        [SerializeField] private Vector2Int _chunkSize;
        [SerializeField] private int _userSeed;
        [SerializeField] private List<Chunk> __whitelistedChunks; //List of all chunks it can use, if its empty it uses all.
        [SerializeField] private List<Chunk> __blacklistedChunks; //List of all chunks it MAY not use, if its empty it uses all or whitelisted.

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