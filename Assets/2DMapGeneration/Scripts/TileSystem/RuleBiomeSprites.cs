using System;
using UnityEngine;

namespace MapGeneration.TileSystem
{
    /// <summary>
    /// This class is used to store biome data for a rule biome tile.
    /// </summary>
    [Serializable]
    public class RuleBiomeSprites
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
        /// The middle sprite.
        /// </summary>
        [SerializeField] public Sprite MiddleSprite;

        /// <summary>
        /// The top sprite.
        /// </summary>
        [SerializeField] public Sprite TopSprite;

        /// <summary>
        /// The left sprite.
        /// </summary>
        [SerializeField] public Sprite LeftSprite;

        /// <summary>
        /// The right sprite.
        /// </summary>
        [SerializeField] public Sprite RightSprite;

    }
}