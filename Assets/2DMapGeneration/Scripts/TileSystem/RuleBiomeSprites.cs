using System;
using UnityEngine;

namespace MapGeneration.TileSystem
{
    /// <summary>
    /// This class is used to store biome data for a rule biome tile
    /// </summary>
    [Serializable]
    public class RuleBiomeSprites
    {
        [SerializeField] public string ID;
        [SerializeField] public Color Tint;
        [SerializeField] public Sprite MiddleSprite;
        [SerializeField] public Sprite TopSprite;
        [SerializeField] public Sprite LeftSprite;
        [SerializeField] public Sprite RightSprite;

        public RuleBiomeSprites()
        {
            Tint = Color.white;
        }
    }
}