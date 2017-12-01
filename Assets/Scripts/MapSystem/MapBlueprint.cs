using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ChunkExtension;
using MapGeneration.Algorithm;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace MapGeneration
{
    /// <summary>
    /// Purpose:
    /// Creator: Peter Witt
    /// </summary>
    [CreateAssetMenu(fileName = "New Map Blueprint", menuName = "MapGeneration/CreateBlueprint")]
    public class MapBlueprint : ScriptableObject
    {
        private readonly List<MapGenerationAlgorithm> _instancedAlgorithms = new List<MapGenerationAlgorithm>();

        //These needs to be serialized and public
        #region Custom Inspector Fields
        [SerializeField] public bool FillEmptySpaces = true;
        [SerializeField] public bool FindValidChunks = true;
        [SerializeField] public bool OpenConnections = true;
        [SerializeField] public List<AlgorithmStorage> AlgorithmStack;
        [SerializeField] public Vector2Int GridSize = new Vector2Int(4, 4);
        [SerializeField] public Vector2Int ChunkSize = new Vector2Int(10, 8);
        [SerializeField] public List<Chunk> WhitelistedChunks; //List of all chunks it can use, if its empty it uses all.
        [SerializeField] public List<Chunk> BlacklistedChunks; //List of all chunks it MAY not use, if its empty it uses all or whitelisted.
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
                usableChunks = WhitelistedChunks.ToList();
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
                usableChunks = usableChunks.Except(BlacklistedChunks).ToList();
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