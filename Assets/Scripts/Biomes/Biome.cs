using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration
{
    public enum TileType
    {
        Edge, Platform
    }

    [CreateAssetMenu(fileName = "Biome", menuName = "MapGeneration/Biome")]
    public class Biome : ScriptableObject
    {
        public RuleBiomeSprites edge;
        public RuleBiomeSprites platform;


        public RuleBiomeSprites GetTileSpite(TileType type)
        {
            RuleBiomeSprites ruleBiomeSprites = new RuleBiomeSprites();
            switch (type)
            {
                case TileType.Edge:
                    ruleBiomeSprites = edge;
                    break;
                case TileType.Platform:
                    ruleBiomeSprites = platform;
                    break;
            }
            return ruleBiomeSprites;
        }
    }


}

