using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapGeneration
{
    public abstract class BiomeTile : TileBase
    {
        [SerializeField] protected Sprite _defaultSprite;
        [SerializeField] protected Tile.ColliderType _colliderType;

        protected Chunk _chunk;
        protected string _biome;

        public override void RefreshTile(Vector3Int position, ITilemap tilemap)
        {
            base.RefreshTile(position, tilemap);
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Vector3Int pos = position + new Vector3Int(x, y, position.z);
                    tilemap.RefreshTile(pos);
                }
            }
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = _defaultSprite;
            tileData.colliderType = _colliderType;
        }

        protected virtual void RefreshBiomeValues(Vector3Int position, ITilemap tilemap)
        {
            //This tries to get a chunk component from the brush target
            GameObject gameObject = tilemap.GetComponent<Tilemap>().gameObject;

            if (gameObject)
            {
                //This tries to get a chunk component from the brush target
                _chunk = gameObject.GetComponent<Chunk>() ??
                         gameObject.GetComponentInParent<Chunk>();

                //if a chunk is found, then get the current biome id and use it to get sprites
                if (_chunk)
                {
                    _biome = NoiseToBiome(_chunk,position);
                    
                }
            }
        }

        private string NoiseToBiome(Chunk chunk, Vector3Int position)
        {
            if(chunk.BiomeValues == null)
                return "";

            if (position.x >= chunk.BiomeValues.GetLength(0) || position.x < 0 ||
                position.y >= chunk.BiomeValues.GetLength(1) || position.y < 0)
            {
                return "";
            }
            return MapBuilder.Settings.NoiseToBiome(chunk.BiomeValues[position.x, position.y]);
        }
    }
}
