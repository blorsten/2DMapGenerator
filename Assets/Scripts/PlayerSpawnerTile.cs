using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Player Spawner Tile", menuName = "MapGeneration/Tiles/Spawner Tile")]
public class SpawnerTile : TileBase
{
    [SerializeField] protected Sprite _sprite;
    [SerializeField] protected GameObject __playerSpawner;

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        Tilemap currentTilemap = tilemap.GetComponent<Tilemap>();

        if (currentTilemap.gameObject.activeInHierarchy && Application.isPlaying)
            Instantiate(__playerSpawner, currentTilemap.GetCellCenterWorld(position),
                Quaternion.identity);
        return base.StartUp(position, tilemap, go);
        
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        tileData.sprite = _sprite;
        tileData.colliderType = Tile.ColliderType.None;
    }
}
