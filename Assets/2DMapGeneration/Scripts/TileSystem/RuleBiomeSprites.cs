using System;
using UnityEngine;

namespace MapGeneration.TileSystem
{
    [Serializable]
    public class RuleBiomeSprites
    {
        [SerializeField] public string iD;
        [SerializeField] public Color tint;
        [SerializeField] public Sprite middleSprite;
        [SerializeField] public Sprite topSprite;
        [SerializeField] public Sprite leftSprite;
        [SerializeField] public Sprite rightSprite;

        public RuleBiomeSprites()
        {
            tint = Color.white;
        }
    }
}