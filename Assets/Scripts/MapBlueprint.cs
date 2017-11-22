using System.Collections.Generic;
using System.Linq;
using ChunkExtension;
using MapGeneration.Algorithm;
using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// Purpose:
    /// Creator: Peter Witt
    /// </summary>
    [CreateAssetMenu(fileName = "new Map Blueprint", menuName = "MapGeneration/CreateBlueprint")]
    public class MapBlueprint : ScriptableObject
    {
        private readonly List<MapGenerationAlgorithm> _instancedAlgorithms = new List<MapGenerationAlgorithm>();

        [SerializeField] private List<MapGenerationAlgorithm> _algorithmStack;
        [SerializeField] private Vector2Int _gridSize = new Vector2Int(4,4);
        [SerializeField] private bool _specifyChunkSize;
        [SerializeField] private Vector2Int _chunkSize;
        [SerializeField] private float _tileSize;
        [SerializeField] private int _userSeed; 
        [SerializeField] private List<Chunk> __whitelistedChunks; //List of all chunks it can use, if its empty it uses all.
        [SerializeField] private List<Chunk> __blacklistedChunks; //List of all chunks it MAY not use, if its empty it uses all or whitelisted.

        public Vector2Int GridSize { get { return _gridSize; }}
        public Vector2Int ChunkSize { get { return _chunkSize; }}
        public float TileSize { get { return _tileSize; }}
        public int UserSeed { get { return _userSeed; } }

        /// <summary>
        /// Generates a map from the blueprint
        /// </summary>
        /// <param name="map">map</param>
        public void Generate(Map map)
        {
            //If we havent instantiated any of the used algorithms, do so.
            _instancedAlgorithms.Clear();
            foreach (var algorithm in _algorithmStack)
                if (algorithm)
                    _instancedAlgorithms.Add(Instantiate(algorithm));

            //Get a list of all the usable chunks.
            List<Chunk> usableChunks = GetUsableChunks();

            //If we got any algorithms, go through them and process them.
            if (_instancedAlgorithms != null && _instancedAlgorithms.Any())
                _instancedAlgorithms.ForEach(algorithm => algorithm.Process(map, usableChunks));
        }

        /// <summary>
        /// Post processes the map after it has been spawned.
        /// </summary>
        /// <param name="map">map</param>
        public void StartPostProcess(Map map)
        {
            //Get a list of all the usable chunks.
            List<Chunk> usableChunks = GetUsableChunks();

            //If we got any algorithms, go through them and post process them.
            if (_instancedAlgorithms != null && _instancedAlgorithms.Any())
                _instancedAlgorithms.ForEach(algorithm => algorithm.PostProcess(map, usableChunks));
        }

        /// <summary>
        /// Grabs all chunks from ResourceHandler and filters them with the settings in this blueprint.
        /// </summary>
        /// <returns>List of usable chunks.</returns>
        public List<Chunk> GetUsableChunks()
        {
            List<Chunk> usableChunks;

            if (__whitelistedChunks != null && __whitelistedChunks.Any())
            {
                //If there are any whitelisted chunks we have to take into account, do so.
                usableChunks = __whitelistedChunks.ToList();
            }
            else
            {
                //We dident have any chunks whitelisted in this blueprint so we use all chunks instead.
                usableChunks = ResourceHandler.Instance.Chunks.ToList();
            }

            //If there is any blacklisted chunks, take them out.
            if (__blacklistedChunks != null && __blacklistedChunks.Any())
            {
                usableChunks = usableChunks.Except(__blacklistedChunks).ToList();
            }

            //Remove all chunks that doesnt fit the blueprint.
            usableChunks.RemoveAll(chunk => !chunk.CompareSize(_chunkSize));

            return usableChunks;
        }
    }
}