using System;
using MapGeneration.ChunkSystem;
using UnityEngine;

namespace MapGeneration.TileSystem
{
    /// <summary>
    /// A enum to determine a FlagTypes
    /// </summary>
    public enum FlagType
    {
        None, Top, Bottom, Left, Right, Trap, Treasure, FlyingSpawn, GroundSpawn
    }

    /// <summary>
    /// This class is to store data outside of a tilemap for tiles
    /// </summary>
    [Serializable]
    public class TileFlag
    {
        [SerializeField] private Vector3Int _position;//The position of the tile in the chunk
        [SerializeField] private FlagType _type;//The type if the tile
        [SerializeField] private Chunk _chunk;//The chunk the tile is contained in

        /// <summary>
        /// The position of the tileflag.
        /// </summary>
        public Vector3Int Position{get { return _position; }set { _position = value; }}

        /// <summary>
        /// The type of tileflag.
        /// </summary>
        public FlagType Type{get { return _type; }set { _type = value; }}

        /// <summary>
        /// The reference to the chunk the tileflag belongs to.
        /// </summary>
        public Chunk Chunk{get { return _chunk; }set { _chunk = value; }}

        /// <summary>
        /// This constructs the tileflag and sets it's values.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="type"></param>
        /// <param name="chunk"></param>
        public TileFlag(Vector3Int position, FlagType type, Chunk chunk)
        {
            Position = position;
            Type = type;
            Chunk = chunk;
        }
       
    }
}


