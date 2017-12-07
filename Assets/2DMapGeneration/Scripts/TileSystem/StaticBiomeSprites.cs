using System;
using UnityEngine;

namespace MapGeneration.TileSystem
{
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


