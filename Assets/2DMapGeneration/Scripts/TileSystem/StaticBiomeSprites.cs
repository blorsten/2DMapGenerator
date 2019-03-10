using System;
using UnityEngine;

namespace MapGeneration.TileSystem
{
    /// <summary>
    /// This class is used to store biome data for a static biome tile
    /// </summary>
    [Serializable]
    public class StaticBiomeSprites
    {
        /// <summary>
        /// The ID is used to determine what biome this belongs to.
        /// </summary>
        [SerializeField] public string ID;

        /// <summary>
        /// A color used to tint the sprite.
        /// </summary>
        [SerializeField] public Color Tint = Color.white;

        /// <summary>
        /// The sprite for the current biome.
        /// </summary>
        [SerializeField] public Sprite Sprite;

    }

}