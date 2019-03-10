using System.Collections.Generic;
using System.Linq;
using MapGeneration.Algorithm;
using MapGeneration.ChunkSystem;
using MapGeneration.Extensions;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace MapGeneration
{
    /// <summary>
    /// Map blueprint is the class that builds a map.
    /// </summary>
    [CreateAssetMenu(fileName = "New Map Blueprint", menuName = "2D Map Generation/Blueprint")]
    public class MapBlueprint : ScriptableObject
    {
        private readonly List<MapGenerationAlgorithm> _instancedAlgorithms = new List<MapGenerationAlgorithm>();

        //These needs to be serialized and public
#region Custom Inspector Fields
        /// <summary>
        /// Should be true if the empty chunks should be filled.
        /// </summary>
        [SerializeField] public bool FillEmptySpaces = true;

        /// <summary>
        /// IF true, then only valid find valid chunks.
        /// </summary>
        [SerializeField] public bool FindValidChunks = true;

        /// <summary>
        /// Should be true if used connections should be open.
        /// </summary>
        [SerializeField] public bool OpenConnections = true;

        /// <summary>
        /// The algorithm stack.
        /// </summary>
        [SerializeField] public List<AlgorithmStorage> AlgorithmStack = new List<AlgorithmStorage>();

        /// <summary>
        /// This cells the mapBlueprint how big the maps grid should be.
        /// </summary>
        [SerializeField] public Vector2Int GridSize = new Vector2Int(4, 4);

        /// <summary>
        /// This tells the mapBlueprint how big the chunks should be.
        /// </summary>
        [SerializeField] public Vector2Int ChunkSize = new Vector2Int(10, 8);

        /// <summary>
        /// List of all chunks it can use, if its empty it uses all.
        /// </summary>
        [SerializeField] public List<Chunk> WhitelistedChunks;

        /// <summary>
        /// List of all chunks it MAY not use, if its empty it uses all or whitelisted.
        /// </summary>
        [SerializeField] public List<Chunk> BlacklistedChunks;

        /// <summary>
        /// A user defined seed.
        /// </summary>
        [SerializeField] public int UserSeed;
#endregion

        /// <summary>
        /// Generates a map from the blueprint 
        /// </summary>
        /// <param name="map">map</param>
        public bool Generate(Map map)
        {
            if (!Validate())
                return false;

            //Get a list of all the usable chunks.
            List<Chunk> usableChunks = GetUsableChunks();
            if (usableChunks == null)
                return false;

            //If we havent instantiated any of the used algorithms, do so.
            _instancedAlgorithms.Clear();
            foreach (var algorithm in AlgorithmStack)
                if (algorithm.Algorithm && algorithm.IsActive)
                    _instancedAlgorithms.Add(Instantiate(algorithm.Algorithm));

            if (FindValidChunks)
                _instancedAlgorithms.Add(CreateInstance<ChunkPlacer>());

            if (OpenConnections)
                _instancedAlgorithms.Add(CreateInstance<ConnectionOpenerAlgorithm>());

            //If we got any algorithms, go through them and process them.
            if (_instancedAlgorithms != null && _instancedAlgorithms.Any())
                foreach (var algorithm in _instancedAlgorithms)
                {
                    if (!algorithm.Process(map, usableChunks))
                    {
                        Debug.LogWarning(
                            string.Format("MapBlueprint: {0} in {1} " +
                                "failed to succeed its process.", algorithm.name,
                                name), this);
                    }
                }

            return true;
        }

        /// <summary>
        /// Post processes the map after it has been spawned.
        /// </summary>
        /// <param name="map">map</param>
        public bool StartPostProcess(Map map)
        {
            if (!Validate())
                return false;

            //Get a list of all the usable chunks.
            List<Chunk> usableChunks = GetUsableChunks();
            if (usableChunks == null)
                return false;

            //If we got any algorithms, go through them and post process them.
            if (_instancedAlgorithms != null && _instancedAlgorithms.Any())
                foreach (var algorithm in _instancedAlgorithms)
                {
                    if (!algorithm.PostProcess(map, usableChunks))
                    {
                        Debug.LogWarning(
                            string.Format("MapBlueprint: {0} in {1} " +
                                "failed to succeed its post process.", algorithm.name,
                                name), this);
                    }
                }

            foreach (var chunk in map.Grid)
            {
                if (chunk.Instance)
                    chunk.Instance.RefreshTilemaps();
            }
            return true;
        }

        /// <summary>
        /// Grabs all chunks from ResourceHandler and filters 
        /// them with the settings in this blueprint.
        /// </summary>
        /// <returns>List of usable chunks.</returns>
        public List<Chunk> GetUsableChunks()
        {
            List<Chunk> usableChunks;

            if (WhitelistedChunks != null && WhitelistedChunks.Any())
            {
                //If there are any whitelisted chunks we have to take into account, do so.
                usableChunks = WhitelistedChunks.Where(chunk => chunk).ToList();
            }
            else
            {
                //We dident have any chunks whitelisted in 
                //this blueprint so we use all chunks instead.
                usableChunks = ResourceHandler.Instance.Chunks.Where(chunk => chunk).ToList();
            }

            //If there is any blacklisted chunks, take them out.
            if (BlacklistedChunks != null && BlacklistedChunks.Any())
            {
                usableChunks = usableChunks.Where(chunk => chunk).Except(BlacklistedChunks).ToList();
            }

            //Remove all chunks that doesnt fit the blueprint.
            usableChunks.RemoveAll(chunk =>
                !chunk.CompareSize(ChunkSize) ||
                !chunk.IsStandaloneChunk);

            if (!usableChunks.Any())
            {
                Debug.LogWarning(string.Format("MapBlueprint: {0} couldn't find any usable " +
                    "chunks, check whitelist/blacklist, or make " +
                    "some suitable chunks.", name), this);
                return null;
            }

            return usableChunks;
        }

        /// <summary>
        /// Validates the blueprint checking if every requirements are met.
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            bool isUsable = true;

            if (GridSize == Vector2Int.zero || GridSize.x == 0 || GridSize.y == 0)
            {
                Debug.LogWarning(string.Format("MapBlueprint: {0} " +
                    "has a invalid grid size.", name), this);
                isUsable = false;
            }

            if (ChunkSize == Vector2Int.zero || ChunkSize.x == 0 || ChunkSize.y == 0)
            {
                Debug.LogWarning(string.Format("MapBlueprint: {0} " +
                    "has a invalid chunk size.", name), this);
                isUsable = false;
            }

            if (AlgorithmStack == null || (AlgorithmStack != null &&
                    AlgorithmStack.All(algorithm => algorithm.Algorithm == null)))
            {
                Debug.LogWarning(string.Format("MapBlueprint: {0} doesn't have any algorithms, " +
                    "make sure to give it some.", name), this);

                isUsable = false;
            }

            return isUsable;
        }
    }
}