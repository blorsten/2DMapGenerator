using System;
using UnityEngine;

namespace MapGeneration
{
    public enum TileType
    {
        TopConnection, BottomConnection, LeftConnection, RightConnection
    }

    /// <summary>
    /// This class is to store data outside of a tilemap for tiles
    /// </summary>
    [Serializable]
    public class Tile
    {
        [SerializeField] private Vector3Int _position;//The postion of the tile in the chunk
        [SerializeField] private TileType _type;//The type if the tile
        [SerializeField] private Chunk _chunk;//The chunk the tile is contained in

        //Properties
        public Vector3Int Position{get { return _position; }set { _position = value; }}
        public TileType Type{get { return _type; }set { _type = value; }}
        public Chunk Chunk{get { return _chunk; }set { _chunk = value; }}

        public Tile(Vector3Int position, TileType type, Chunk chunk)
        {
            Position = position;
            Type = type;
            Chunk = chunk;
        }
       
    }
}


