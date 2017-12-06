using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MapGeneration.SaveSystem;
using MapGeneration.Utils;
using UnityEditor;
using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// Root of the whole system, this manager/builder generates, saved & loads map.
    /// </summary>
    public class MapBuilder : Singleton<MapBuilder>
    {
        private List<MapDataSaver> _savedMaps;
        private static MapGenerationSettings _settings;

        [SerializeField] private MapBlueprint _currentBlueprint;
        [SerializeField] private Map _preExistingMap;
        [SerializeField] private List<int> _savedSeeds = new List<int>();

        public Map ActiveMap { get; set; }

        public MapBlueprint CurrentBlueprint { get { return _currentBlueprint; } set { _currentBlueprint = value; } }
        public Map PreExistingMap { get { return _preExistingMap; } set { _preExistingMap = value; } }
        public List<int> SavedSeeds { get { return _savedSeeds; } set { _savedSeeds = value; } }

        public List<MapDataSaver> SavedMaps { get { return _savedMaps ?? (_savedMaps = new List<MapDataSaver>()); } }
        public static MapGenerationSettings Settings { get { return _settings ?? (_settings = AssetDatabase.LoadAssetAtPath<MapGenerationSettings>("Assets/2DMapGeneration/MapGenerationSettings.asset")); } }

        /// <summary>
        /// Event is invoked a frame after a map has been spawned.
        /// </summary>
        public event Action<Map> MapSpawned;

        protected override void Awake()
        {
            base.Awake();
            
            //If we generated a map in editor make it active when running the game.
            if (PreExistingMap)
            {
                Generate(PreExistingMap.MapBlueprint, PreExistingMap.Seed);
                Despawn(PreExistingMap);
            }
        }

        /// <summary>
        /// Generates a map from a specific blueprint
        /// </summary>
        /// <param name="mapBlueprint">blueprint</param>
        /// <param name="seed">If defined it will be the chosen seed for this generation.</param>
        /// <returns>Map</returns>
        public Map Generate(MapBlueprint mapBlueprint, int seed = 0)                               
        {
            if (!mapBlueprint)
            {
                Debug.LogWarning("MapBuilder: Tried to generate map from " +
                               "blueprint but diden't get one.", gameObject);
                return null;
            }
            
            //If the seed has been defined in the blueprint use that instead.
            int chosenSeed;

            if (seed != 0)
                chosenSeed = seed;
            else if (mapBlueprint.UserSeed != 0)
                chosenSeed = mapBlueprint.UserSeed;
            else
                chosenSeed = DateTime.Now.Millisecond;

            //Creating the new map.
            Map map = new GameObject(mapBlueprint.name).AddComponent<Map>();
            map.Tilemaps.Clear();
            map.gameObject.AddComponent<MapGizmos>().Map = map;
            map.Initialize(chosenSeed, mapBlueprint);

            //Start the blueprint process.
            if (!mapBlueprint.Generate(map))
            {
                CleanupFailedMap(map);
                return null;
            }

            //Save the new map.
            Save(map);

            //Now that the map is fully made, spawn it.
            Spawn(map);

            ActiveMap = map;

            return map;
        }

        public Map Generate(MapDataSaver existingMap)
        {
            if (existingMap == null)
            {
                Debug.LogWarning("MapBuilder: Tried to generate map from " +
                                 "a existing map but diden't get a valid one.", gameObject);
                return null;
            }

            //Creating the new map.
            Map map = new GameObject(existingMap.MapBlueprint.name).AddComponent<Map>();

            //If save data from active map before making this new one.
            if (ActiveMap.MapDataSaver == existingMap && existingMap.Map)
                existingMap.SavePersistentData();

            //Update MapDataSaver with the new map reference.
            existingMap.Map = map;

            //Initialize the map with the existing data saver.
            map.Initialize(existingMap.MapSeed, existingMap.MapBlueprint, existingMap);

            //Start the blueprint process.
            if (!existingMap.MapBlueprint.Generate(map))
            {
                CleanupFailedMap(map);
                return null;
            }

            //Now that the map is fully made, spawn it.
            Spawn(map);
            return map;
        }

        /// <summary>
        /// Generates a map from current blueprint
        /// </summary>
        /// <returns>Map</returns>
        public Map Generate()
        {
            return Generate(CurrentBlueprint);
        }

        /// <summary>
        /// Generates a map from current blueprint and a specified seed.
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public Map Generate(int seed)
        {
            return Generate(CurrentBlueprint, seed);
        }

        /// <summary>
        /// Saves a map
        /// </summary>
        /// <param name="map">map</param>
        public void Save(Map map)
        {
            SavedMaps.Add(map.MapDataSaver);
        }

        /// <summary>
        /// Spawns a map as instances
        /// </summary>
        /// <param name="map">map</param>
        public void Spawn(Map map)
        {
            Vector2 gridSize = map.MapBlueprint.GridSize;
            Vector2 chunkSize = map.MapBlueprint.ChunkSize;
            
            //Remember if we have a already active map.
            Map oldMap = ActiveMap;

            //Set the new map as active.
            ActiveMap = map;

            //Lets destroy the old map if there was one.
            if (oldMap != null)
                Despawn(oldMap);

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    float xPosition = map.transform.position.x + chunkSize.x * x;
                    float yPosition = map.transform.position.y + chunkSize.y * y;

                    if (map.Grid[x, y] != null && map.Grid[x,y].Prefab != null) 
                        map.Grid[x, y].Instantiate(new Vector2(xPosition, yPosition), 
                            map.transform,map);

                }
            }

            //Start the post process
            map.MapBlueprint.StartPostProcess(map);

            map.MapDataSaver.LoadPersistentData();

            StartCoroutine(InvokeMapSpawned(ActiveMap));
        }

        /// <summary>
        /// Despawns a map from the world
        /// </summary>
        /// <param name="map">map</param>
        public void Despawn(Map map)
        {
            //If the new map isn't the same as the old one, save its data before despawning.
            if (map && map.MapDataSaver != null && map.MapDataSaver != ActiveMap.MapDataSaver)
                map.MapDataSaver.SavePersistentData();

            if (Application.isPlaying)
            {
                //Destroying all instances of the spawned chunks
                Destroy(map.gameObject);
            }
            else
                DestroyImmediate(map.gameObject);
        }

        private void CleanupFailedMap(Map map)
        {
            Debug.LogWarning(string.Format("MapBuilder: {0} failed to generate.", map.name), this);

            if (map)
            {
                if (!Application.isPlaying)
                    DestroyImmediate(map.gameObject);
                else
                    Destroy(map.gameObject);
            }
        }

        /// <summary>
        /// Invokes the <see cref="MapSpawned"/> event a frame after this method has been called.
        /// </summary>
        /// <param name="map">The map that has been spawned</param>
        private IEnumerator InvokeMapSpawned(Map map)
        {
            yield return null;
            if (MapSpawned != null) MapSpawned.Invoke(map);
        }
    }
}
