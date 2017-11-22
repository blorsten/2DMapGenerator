using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration
{
    public enum ObjectType
    {
        Treasure, Trap, FlyingSpawner, GroundSpawner
    }

    public class MapObject : MonoBehaviour
    {
        [SerializeField] private ObjectType _type;

        public ObjectType Type{get { return _type; }}
    }
}


