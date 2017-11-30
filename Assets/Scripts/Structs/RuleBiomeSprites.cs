using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration
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
    }
}