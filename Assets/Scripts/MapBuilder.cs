using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration
{
    public class MapBuilder : Singleton<MapBuilder>
    {
        [SerializeField] private MapBlueprint _currentBlueprint;

        [SerializeField] private ResourceHandler _resourceHandler;

        public Map ActiveMap { get; set; }
        public List<Map> Maps { get; set; }

        /// <summary>
        /// Generates a map from a specific blueprint
        /// </summary>
        /// <param name="mapBlueprint">blueprint</param>
        /// <returns>Map</returns>
        public Map Generate(MapBlueprint mapBlueprint)
        {
            return null;
        }

        /// <summary>
        /// Generates a map form current blueprint
        /// </summary>
        /// <returns>Map</returns>
        public Map Generate()
        {
            return null;
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

        }

        /// <summary>
        /// Despawns a map from the world
        /// </summary>
        /// <param name="map">map</param>
        public void Despawn(Map map)
        {
            
        }
    }
}