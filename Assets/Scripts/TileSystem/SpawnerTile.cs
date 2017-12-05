using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapGeneration.TileSystem
{
    /// <summary>
    /// Custom tile that spawns a object on start.
    /// </summary>
    [CreateAssetMenu(fileName = "Spawner Tile", menuName = "MapGeneration/Tiles/Spawner Tile")]
    public class SpawnerTile : TileBase
    {
        [SerializeField] protected Sprite Sprite;
        [SerializeField] protected GameplayObject ObjectToSpawn;

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