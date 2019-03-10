using System;
using MapGeneration.Extensions;
using UnityEngine;
using Random = System.Random;

namespace MapGeneration.SaveSystem
{
    /// <summary>
    /// This class used to find certain game objects that needs saving.
    /// </summary>
    public class DataIdentity : MonoBehaviour
    {
        /// <summary>
        /// The data's id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The data's Random.
        /// </summary>
        public Random Random { get; set; }

        /// <summary>
        /// The data's dirty status.
        /// </summary>
        public bool IsDirty { get; set; }

        /// <summary>
        /// Make the data identity ready for loading and saving by creating its id.
        /// </summary>
        /// <param name="random"></param>
        /// <param name="guid"></param>
        public virtual void Initialize(Random random, Guid guid = default(Guid))
        {
            Random = random;
            Id = guid == default(Guid) ? new Guid(RandomExtension.GenerateByteSeed(random)) : guid;
        }
    }
}