using System.Collections.Generic;
using System.Linq;
using MapGeneration.ChunkSystem;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapGeneration.TileSystem
{
    /// <summary>
    /// This tile will change it's sprite relative to it's neighbors 
    /// </summary>
    [CreateAssetMenu(fileName = "New Rule Biome Tile", menuName = "2D Map Generation/Tiles/Rule Biome")]
    public class RuleBiomeTile : BiomeTile
    {
        /// <summary>
        /// This enum is used to determine the neighbor type
        /// </summary>
        private enum TileNeighborType
        {
            Middle,
            top,
            bottom,
            left,
            Right
        }
         
        [SerializeField] private List<RuleBiomeSprites> _biomesSprites 
            = new List<RuleBiomeSprites>();

        private RuleBiomeSprites _currentSprites;
        private TileNeighborType _neighborType;

        /// <summary>
        /// This is called when the tile is refreshed
        /// </summary>
        /// <param name="position"></param>
        /// <param name="tilemap"></param>
        protected override void RefreshBiomeValues(Vector3Int position, ITilemap tilemap)
        {
            base.RefreshBiomeValues(position, tilemap);

                //if a chunk is found, then get the current biome id and use it to get sprites
                if (_chunk)
                {
                    _currentSprites = _biomesSprites.Count > 0
                        ? _biomesSprites.FirstOrDefault(x => x.iD == _biome)
                        : null;

                    //This checks the tiles neighbor status
                    _neighborType = GetNeighborType(position, tilemap);
            }
            
        }

        /// <summary>
        /// This set the tile's data which are used to draw it
        /// </summary>
        /// <param name="position"></param>
        /// <param name="tilemap"></param>
        /// <param name="tileData"></param>
        public override void GetTileData(Vector3Int position, ITilemap tilemap,
            ref TileData tileData)
        {
            base.GetTileData(position,tilemap,ref tileData);

            //This refeshes the values needed for the tile
            RefreshBiomeValues(position, tilemap);

            if (_currentSprites != null && _currentSprites.iD != "")
            {
                switch (_neighborType)
                {
                    case TileNeighborType.Middle:
                        tileData.sprite = _currentSprites.middleSprite;
                        break;
                    case TileNeighborType.top:
                        tileData.sprite = _currentSprites.topSprite;
                        break;
                    case TileNeighborType.left:
                        tileData.sprite = _currentSprites.leftSprite;
                        break;
                    case TileNeighborType.Right:
                        tileData.sprite = _currentSprites.rightSprite;
                        break;
                    case TileNeighborType.bottom:
                        tileData.sprite = _currentSprites.middleSprite;
                        break;

                }
                /*TODO Figure out why setting the tiledata.color dosen't change the color of the tile 
                and why this does.*/
                tilemap.GetComponent<Tilemap>().SetColor(position, _currentSprites.tint);
            }            
        }

        /// <summary>
        /// Call this to get the current neighbor status
        /// </summary>
        /// <param name="position"></param>
        /// <param name="tilemap"></param>
        /// <returns></returns>
        private TileNeighborType GetNeighborType(Vector3Int position, ITilemap tilemap)
        {
            TileNeighborType type = 0;
            TileBase top = null;
            TileBase left = null;
            TileBase right = null;

            bool topOutOfBounds =
                !CheckNextChunk(position, new Vector3Int(0, 1, 0), ref top, tilemap);

            CheckNextChunk(position, new Vector3Int(-1, 0, 0), ref left, tilemap);
            CheckNextChunk(position, new Vector3Int(1, 0, 0), ref right, tilemap);

            if (top && (left || right) || topOutOfBounds)
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

        /// <summary>
        /// Call this to check if the tile in the direction is in the current tilemap and if not
        /// then return the tile in the next tilemap in the direction. Returns false if its out
        /// of bounds of the chunk grid
        /// </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        /// <param name="tile"></param>
        /// <param name="tilemap"></param>
        /// <returns></returns>
        private bool CheckNextChunk(Vector3Int position, Vector3Int direction, ref TileBase tile, ITilemap tilemap)
        {
            bool hasMap = _chunk != null && _chunk.Map != null;


            if ((position.x + direction.x >= _chunk.Width || position.x + direction.x < 0
                || position.y + direction.y >= _chunk.Height || position.y + direction.y < 0 )
                && hasMap)
            {
                Vector2Int newPosition = _chunk.ChunkHolder.Position + new Vector2Int(direction.x,direction.y);
                ChunkHolder chunkholder = _chunk.Map.GetChunkHolder(newPosition);
                if (chunkholder != null && chunkholder.Instance && chunkholder.Instance.Environment)
                {
                    tile = chunkholder.Instance.Environment.GetTile(new Vector3Int(0, position.y,
                        position.z));
                }
                else
                    return false;

            }
            else
                tile = tilemap.GetTile(position + direction);
            return true;
        }

        
    }
}