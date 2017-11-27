using System;
using UnityEngine;

namespace MapGeneration
{
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
        [SerializeField] private Vector3Int _position;//The postion of the tile in the chunk
        [SerializeField] private FlagType _type;//The type if the tile
        [SerializeField] private Chunk _chunk;//The chunk the tile is contained in

        //Properties
        public Vector3Int Position{get { return _position; }set { _position = value; }}
        public FlagType Type{get { return _type; }set { _type = value; }}
        public Chunk Chunk{get { return _chunk; }set { _chunk = value; }}

        public TileFlag(Vector3Int position, FlagType type, Chunk chunk)
        {
            Position = position;
            Type = type;
            Chunk = chunk;
        }
       
    }
}


