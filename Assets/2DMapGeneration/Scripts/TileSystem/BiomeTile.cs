using MapGeneration.ChunkSystem;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapGeneration.TileSystem
{
    /// <summary>
    /// This is the root class for all biome related tiles.
    /// </summary>
    public abstract class BiomeTile : TileBase
    {
        [SerializeField] protected Sprite _defaultSprite;
        [SerializeField] protected Tile.ColliderType _colliderType;

        protected Chunk _chunk;
        protected string _biome;
        protected GameObject _gameObject;

        /// <summary>
        /// This is called when a tile is refreshed and it
        /// </summary>
        /// <param name="position"></param>
        /// <param name="tilemap"></param>
        public override void RefreshTile(Vector3Int position, ITilemap tilemap)
        {
            base.RefreshTile(position, tilemap);

            //This refreshes the tiles neighbors
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
            _gameObject = tilemap.GetComponent<Tilemap>().gameObject;

            //This method is used to add the gameobject to a dictionary if it's is not present,
            //this is done because the alternative was to getcomponent to first get the gameobject 
            // and then the tilemap, this way is much faster
            CheckTilemaps(_gameObject);

            //Here the biome values are reset and if the gameobject is present in the active map's
            //tilemap dictionary, then get the values
            _biome = ""; 
            _chunk = null;
            if (MapBuilder.Instance && MapBuilder.Instance.ActiveMap &&
                MapBuilder.Instance.ActiveMap.Tilemaps.ContainsKey(_gameObject))
            {
                _chunk = MapBuilder.Instance.ActiveMap.Tilemaps[_gameObject]; 
                _biome = NoiseToBiome(_chunk, position);
            }
        } 

        /// <summary>
        /// THis is used to check if the gameobjects if not in the tilemaps dictionary in the 
        /// active map, if not, then add the gameobect and it's chunk
        /// </summary>
        /// <param name="go"></param>
        private void CheckTilemaps(GameObject go)
        {
            if (MapBuilder.Instance == null || MapBuilder.Instance.ActiveMap == null || go == null)
                return;
            if (!MapBuilder.Instance.ActiveMap.Tilemaps.ContainsKey(go))
            {
                _chunk = go.GetComponentInParent<Chunk>();
                if (_chunk)
                    MapBuilder.Instance.ActiveMap.Tilemaps.Add(go, _chunk);
            }
        }

        /// <summary>
        /// Call this to gety the noise value
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="position"></param>
        /// <returns></returns>
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
