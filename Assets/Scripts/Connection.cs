using System;
using UnityEngine;

namespace MapGeneration
{
    [Serializable]
    public class Connection
    {
        [SerializeField] private Vector3Int _position;
        [SerializeField] private ConnectionType _type;
        [SerializeField] private Chunk _chunk;


        public Vector3Int Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public ConnectionType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public Chunk Chunk
        {
            get { return _chunk; }
            set { _chunk = value; }
        }


        public Connection(Vector3Int position, ConnectionType type, Chunk chunk)
        {
            Position = position;
            Type = type;
            Chunk = chunk;
        }
       
    }
}


