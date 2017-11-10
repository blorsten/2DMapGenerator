using System;
using MapGeneration.Extensions;
using UnityEngine;
using Random = System.Random;

namespace MapGeneration.SaveSystem
{
    /// <summary>
    /// Purpose: Used to find certain game objects that needs saving.
    /// Creator: MP
    /// </summary>
    public class DataIdentity : MonoBehaviour 
    {
        public Guid Id { get; set; }
        public Random Random { get; set; }
        public bool IsDirty { get; set; }

        public virtual void Initialize(Random random, Guid guid = default(Guid))
        {
            Random = random;
            Id = guid == default(Guid) ? new Guid(RandomExtension.GenerateByteSeed(random)) : guid;
        }
    }
}