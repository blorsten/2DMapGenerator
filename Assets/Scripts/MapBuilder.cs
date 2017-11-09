using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace MapGeneration
{
    public class MapBuilder : Singleton<MapBuilder>
    {
        [SerializeField] private MapBlueprint _currentBlueprint;

        public Map ActiveMap { get; set; }
        public List<Map> Maps { get; set; }

        /// <summary>
        /// Generates a map from a specific blueprint
        /// </summary>
        /// <param name="mapBlueprint">blueprint</param>
        /// <returns>Map</returns>
        public Map Generate(MapBlueprint mapBlueprint)
        {
            //If the seed has been defined in the blueprint use that instead
            var seed = mapBlueprint.UserSeed != 0 ? 
                mapBlueprint.UserSeed : 
                DateTime.Now.Millisecond;

            //Create a new random from that seed.
            Random random = new Random(seed);

            //Creating the new map
            Map map = new GameObject(mapBlueprint.name).AddComponent<Map>();
            map.Initialize(seed,mapBlueprint,random);

            Maps = new List<Map> {map};

            //Start the blueprint process
            mapBlueprint.Generate(map);

            //Now that the map is fully made, spawn it.
            Spawn(map);

            //Start the post process
            mapBlueprint.StartPostProcess(map);

            ActiveMap = map;

            return map;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
                Despawn(ActiveMap);
        }

        /// <summary>
        /// Generates a map form current blueprint
        /// </summary>
        /// <returns>Map</returns>
        public Map Generate()
        {
            return Generate(_currentBlueprint);
        }

        /// <summary>
        /// Saves a map
        /// </summary>
        /// <param name="map">map</param>
        public void Save(Map map)
        {

        }

        /// <summary>
        /// Spawns a map as instances
        /// </summary>
        /// <param name="map">map</param>
        public void Spawn(Map map)
        {
            float chunkSizeX = map.MapBlueprint.ChunkSize.x;
            float chunkSizeY = map.MapBlueprint.ChunkSize.y;

            for (int x = 0; x < map.MapBlueprint.GridSize.x; x++)
            {
                for (int y = 0; y < map.MapBlueprint.GridSize.y; y++)
                {
                    float xPosition = map.transform.position.x + chunkSizeX * x;
                    float yPOsition = map.transform.position.y + chunkSizeY * y;

                    if (map.Grid[x, y] != null && map.Grid[x,y].Prefab != null) 
                        map.Grid[x, y].Instantiate(new Vector2(xPosition, yPOsition), transform);
                }
            }
        }

        /// <summary>
        /// Despawns a map from the world
        /// </summary>
        /// <param name="map">map</param>
        public void Despawn(Map map)
        {
            //todo: Save persistant data before destroying

            //Destroying all instances of the spawned chunks
            for (int i = 0; i < map.Grid.GetLength(0); i++)
            {
                for (int j = 0; j < map.Grid.GetLength(1); j++)
                {
                    if(map.Grid[i,j].Instance)
                        Destroy(map.Grid[i,j].Instance.gameObject);
                }
            }
        }


        
    }
}
