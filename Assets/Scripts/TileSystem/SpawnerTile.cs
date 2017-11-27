using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapGeneration.TileSystem
{
    [CreateAssetMenu(fileName = "Spawner Tile", menuName = "MapGeneration/Tiles/Spawner Tile")]
    public class SpawnerTile : TileBase
    {
        [SerializeField] protected Sprite Sprite;
        [SerializeField] protected GameplayObject ObjectToSpawn;

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            Tilemap currentTilemap = tilemap.GetComponent<Tilemap>();

            if (currentTilemap.gameObject.activeInHierarchy && Application.isPlaying)
            {
                if (!ObjectToSpawn)
                {
                    Debug.LogWarning(string.Format("SpawnTile: {0} is missing a object to spawn reference.", name), this);
                    return false;
                }

                GameplayObject newObj = Instantiate(ObjectToSpawn, currentTilemap.GetCellCenterWorld(position),
                    Quaternion.identity, currentTilemap.transform);

                newObj.Owner = currentTilemap.GetComponentInParent<Chunk>();
            }

            return base.StartUp(position, tilemap, go);
        
        } 

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);
            tileData.sprite = Application.isPlaying ? null : Sprite;
            tileData.colliderType = Tile.ColliderType.None;
        }
    }
}