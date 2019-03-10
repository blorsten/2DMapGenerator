using MapGeneration.ChunkSystem;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapGeneration.TileSystem
{
    /// <summary>
    /// Custom tile that spawns an object on start.
    /// </summary>
    [CreateAssetMenu(fileName = "New Spawner Tile", menuName = "2D Map Generation/Tiles/Spawner")]
    public class SpawnerTile : TileBase
    {
        [SerializeField] protected Sprite Sprite;
        [SerializeField] protected GameplayObject ObjectToSpawn;

        /// <summary>
        /// This spawns a object
        /// </summary>
        /// <param name="position"></param>
        /// <param name="tilemap"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            //Grabs the tilemap that this tile belongs to.
            Tilemap currentTilemap = tilemap.GetComponent<Tilemap>();

            //If the game is playing and we have a valid tilemap, position the spawned gameobject.
            if (currentTilemap.gameObject.activeInHierarchy && Application.isPlaying &&
                go != null)
            {
                go.transform.position = currentTilemap.GetCellCenterWorld(position);
                go.transform.parent = currentTilemap.transform;
                GameplayObject gameplayObject = go.GetComponent<GameplayObject>();
                if (gameplayObject != null)
                    gameplayObject.Owner = currentTilemap.GetComponentInParent<Chunk>();
            }

            return base.StartUp(position, tilemap, go);
        }

        /// <summary>
        /// This sets the tiles data, which are used to draw the tile
        /// </summary>
        /// <param name="position"></param>
        /// <param name="tilemap"></param>
        /// <param name="tileData"></param>
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            if (!ObjectToSpawn)
                Debug.LogWarning(string.Format("SpawnTile: {0} is missing a object to spawn reference.", name), this);
            else if (Application.isPlaying && tileData.gameObject == null)
                tileData.gameObject = ObjectToSpawn.gameObject;

            tileData.sprite = Application.isPlaying ? null : Sprite;
            tileData.colliderType = Tile.ColliderType.None;
        }
    }
}