using System;
using System.Collections.Generic;
using System.Linq;
using MapGeneration.SaveSystem;
using MapGeneration.Utils;
using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// This class is used to get a game flow.
    /// </summary>
    public class MapCycler : Singleton<MapCycler>
    {
        //Used to mark if the next end/start chunks should grab the player.
        private bool _isStartChunk;

        //Is it the map cyclers job to start the game or a third party?
        [SerializeField] private bool _autoStart;
        [SerializeField] private GameObject __player;

        /// <summary>
        /// A reference to the player.
        /// </summary>
        public GameObject Player { get; set; }

        /// <summary>
        /// A list contained IDs for maps.
        /// </summary>
        public LinkedList<Guid> Maps { get; set; }

        /// <summary>
        /// A reference to the current map.
        /// </summary>
        public LinkedListNode<Guid> CurrentMap { get; set; }

        protected override void Awake()
        {
            base.Awake();
            Maps = new LinkedList<Guid>();
            MapBuilder.Instance.MapSpawned += OnMapSpawned;
        }

        void Start()
        {
            if (!MapBuilder.Instance.SavedMaps.Any() && _autoStart)
                LoadNextMap();
        }

        private void OnMapSpawned(Map activeMap)
        {
            //If we generated a preexisting map, count it as one in the cycler.
            if (!Maps.Contains(activeMap.ID))
            {
                CurrentMap = Maps.AddLast(activeMap.ID);
                _isStartChunk = true;
            }

            GrabPlayer(activeMap, _isStartChunk);
        }

        /// <summary>
        /// Loads the next map in the linked list.
        /// </summary>
        public void LoadNextMap()
        {
            //If we havent generated our first map
            if (CurrentMap == null || CurrentMap.Next == null)
            {
                Generate(Guid.Empty, true);
            }
            else
            {
                Generate(CurrentMap.Next.Value, true);
            }
        }

        /// <summary>
        /// Loads the previous map in the linked list.
        /// </summary>
        public void LoadPreviousMap()
        {
            //If we're trying to load a map that isnt' there, dont.
            if (Maps == null || (Maps != null && CurrentMap.Previous == null))
            {
                Debug.LogWarning(string.Format("MapCycler: {0} tried to load a previous map, " +
                    "but it isn't there.", name), this);
                return;
            }

            if (CurrentMap.Previous != null) Generate(CurrentMap.Previous.Value, false);
        }

        /// <summary>
        /// Generates a new map if it needs to load a non-existing map.
        /// </summary>
        /// <param name="id">ID of a preexisting map if any.</param>
        /// <param name="isStartChunk">Should it place the player in start chunk?</param>
        public void Generate(Guid id, bool isStartChunk)
        {
            MapDataSaver foundMap = MapBuilder.Instance.SavedMaps.FirstOrDefault(saver => saver.MapId == id);
            if (foundMap != null)
            {
                CurrentMap = Maps.Find(id);
                MapBuilder.Instance.Generate(foundMap);
            }
            else
            {
                var newMap = MapBuilder.Instance.Generate();
                CurrentMap = Maps.AddLast(newMap.ID);
            }

            _isStartChunk = isStartChunk;
        }

        /// <summary>
        /// Takes a map and places a player on it.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="isStartChunk">Should it place the player in start chunk?</param>
        public void GrabPlayer(Map map, bool isStartChunk)
        {
            if (!Player)
                Player = Instantiate(__player);
            if (isStartChunk)
            {
                if (map.StartChunk.Instance)
                {
                    PlayerSpawner plySpawner = map.StartChunk.Instance.GetComponentInChildren<PlayerSpawner>();
                    plySpawner.GrabPlayer(Player);
                }
            }
            else
            {
                if (map.EndChunk.Instance)
                {
                    PlayerSpawner plySpawner = map.EndChunk.Instance.GetComponentInChildren<PlayerSpawner>();
                    plySpawner.GrabPlayer(Player);
                }
            }
        }
    }
}