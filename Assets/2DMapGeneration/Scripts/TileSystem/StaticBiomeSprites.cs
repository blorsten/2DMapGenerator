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
        [SerializeField] public string iD;
        [SerializeField] public Color tint;
        [SerializeField] public Sprite sprite;

        public StaticBiomeSprites()
        {
            tint = Color.white;
        }
    }

}


