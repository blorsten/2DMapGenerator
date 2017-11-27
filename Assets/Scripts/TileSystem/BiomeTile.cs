using System;
using System.Collections;
using System.Collections.Generic;
using MapGeneration;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "BiomeTile", menuName = "MapGeneration/Tiles/BiomeTile")]
public class BiomeTile : TileBase
{
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private TileType _tileType;

    private enum TileNeighborType
    {
        Middle, top, bottom, left, Right
    }

    private Biome _biome;
    private Chunk _chunk;
    private TileSprite _tileSprite;

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
        //This tries to get a chunk component from the brush target
        GameObject gameObject = tilemap.GetComponent<Tilemap>().gameObject;

        if (gameObject)
        {
            //This tries to get a chunk component from the brush target
            _chunk = gameObject.GetComponent<Chunk>() ??
                          gameObject.GetComponentInParent<Chunk>();
            _biome = GetBiome();
            _tileSprite = _biome.GetTileSpite(_tileType);
        }
        //for (int x = -1; x < 2; x += 2)
        //{
        //    for (int y = -1; y < 2; y += 2)
        //    {
        //        Vector3Int pos = new Vector3Int(x, y, position.z);
        //        TileBase tile = tilemap.GetTile(pos);
        //        if (tile)
        //        tile.RefreshTile(pos,tilemap);
        //    }
        //}
    }


    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        if (_biome)
        {
            TileNeighborType neighborType = GetNeightbotType(position, tilemap);
            switch (neighborType)
            {
                case TileNeighborType.Middle:
                    tileData.sprite = _tileSprite.middleSprite;
                    break;
                case TileNeighborType.top:
                    tileData.sprite = _tileSprite.topSprite;
                    break;
                case TileNeighborType.left:
                    tileData.sprite = _tileSprite.leftSprite;
                    break;
                case TileNeighborType.Right:
                    tileData.sprite = _tileSprite.rightSprite;
                    break;
                case TileNeighborType.bottom:
                    tileData.sprite = _tileSprite.middleSprite;
                    break;
            }
        }
        else
            tileData.sprite = _defaultSprite;


    }

    private TileNeighborType GetNeightbotType(Vector3Int position, ITilemap tilemap)
    {
        TileNeighborType type = 0;
        TileBase top = tilemap.GetTile(position + new Vector3Int(0, 1, 0));
        TileBase left = tilemap.GetTile(position + new Vector3Int(-1, 0, 0));
        TileBase right = tilemap.GetTile(position + new Vector3Int(1, 0, 0));

        if (top && (left || right))
            type = TileNeighborType.Middle;

        else if (!top && right && !left)
            type = TileNeighborType.left;

        else if (!top && left && !right)
            type = TileNeighborType.Right;

        else if (top)
            type = TileNeighborType.bottom;

        else if (!top)
            type = TileNeighborType.top;

        return type;
    }

    private Biome GetBiome()
    {
        return _chunk.biome;
    }


}