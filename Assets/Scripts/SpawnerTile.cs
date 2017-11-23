using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Spawner Tile", menuName = "MapGeneration/Tiles/Spawner Tile")]
public class SpawnerTile : TileBase
{
    [SerializeField] protected Sprite _sprite;
    [SerializeField] protected GameObject __objectToSpawn;

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        Tilemap currentTilemap = tilemap.GetComponent<Tilemap>();

        if (currentTilemap.gameObject.activeInHierarchy && Application.isPlaying)
        {
            if (!__objectToSpawn)
            {
                Debug.LogWarning(string.Format("SpawnTile: {0} is missing a object to spawn reference.", name), this);
                return false;
            }

            Instantiate(__objectToSpawn, currentTilemap.GetCellCenterWorld(position),
                Quaternion.identity);
        }

        return base.StartUp(position, tilemap, go);
        
    } 

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        tileData.sprite = Application.isPlaying ? null : _sprite;
        tileData.colliderType = Tile.ColliderType.None;
    }
}
