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
        public TileSprite edge;
        public TileSprite platform;


        public TileSprite GetTileSpite(TileType type)
        {
            TileSprite tileSprite = new TileSprite();
            switch (type)
            {
                case TileType.Edge:
                    tileSprite = edge;
                    break;
                case TileType.Platform:
                    tileSprite = platform;
                    break;
            }
            return tileSprite;
        }
    }


}

