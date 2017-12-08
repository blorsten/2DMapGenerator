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
        [SerializeField] public string ID;
        [SerializeField] public Color Tint;
        [SerializeField] public Sprite Sprite;

        public StaticBiomeSprites()
        {
            Tint = Color.white;
        }
    }

}


