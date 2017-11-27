using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TileSprite
{
    [SerializeField] public Sprite middleSprite;
    [SerializeField] public Sprite topSprite;
    [SerializeField] public Sprite leftSprite;
    [SerializeField] public Sprite rightSprite;
}
