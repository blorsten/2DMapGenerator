using System;
using UnityEngine;

namespace MapGeneration
{
    [Serializable]
    public class Connection
    {
        [SerializeField]
        private Vector3Int _position;
        [SerializeField]
        private ConnectionType _type;

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


        public Connection(Vector3Int position, ConnectionType type)
        {
            Position = position;
            Type = type;
        }
       
    }
}


