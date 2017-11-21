using System;
using System.Collections.Generic;
using MapGeneration.SaveSystem;
using UnityEngine;

namespace MapGeneration
{
    public class MapBuilder : Singleton<MapBuilder>
    {
        private List<MapDataSaver> _savedMaps;

        [SerializeField] private MapBlueprint _currentBlueprint;
        [SerializeField] private Map _preExistingMap;
        [SerializeField] private List<int> _savedSeeds;

        public Map ActiveMap { get; set; }

        public MapBlueprint CurrentBlueprint { get { return _currentBlueprint; } set { _currentBlueprint = value; } }
        public Map PreExistingMap { get { return _preExistingMap; } set { _preExistingMap = value; } }
        public List<int> SavedSeeds { get { return _savedSeeds; } set { _savedSeeds = value; } }
        public List<MapDataSaver> SavedMaps { get { return _savedMaps ?? (_savedMaps = new List<MapDataSaver>()); } }

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
                Debug.LogError("MapBuilder: Tried to generate map from blueprint but diden't get one!", gameObject);
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
            map.Initialize(chosenSeed, mapBlueprint);

            //Save the new map.
            Save(map);

            //Start the blueprint process.
            mapBlueprint.Generate(map);

            //Now that the map is fully made, spawn it.
            Spawn(map);

            ActiveMap = map;

            return map;
        }

        public Map Generate(MapDataSaver existingMap)
        {
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
            existingMap.MapBlueprint.Generate(map);

            //Now that the map is fully made, spawn it.
            Spawn(map);
            return map;
        }

        /// <summary>
        /// Generates a map form current blueprint
        /// </summary>
        /// <returns>Map</returns>
        public Map Generate()
        {
            return Generate(CurrentBlueprint);
        }

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
                    float xPosition = transform.position.x + chunkSize.x * x
                        + chunkSize.x / 2;
                    float yPosition = transform.position.y + chunkSize.y * y
                        + chunkSize.y / 2;

                    if (map.Grid[x, y] != null && map.Grid[x,y].Prefab != null) 
                        map.Grid[x, y].Instantiate(new Vector2(xPosition, yPosition), 
                            map.transform);
                }
            }

            //Start the post process
            map.MapBlueprint.StartPostProcess(map);

            map.MapDataSaver.LoadPersistentData();

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
    }
}
