using System.Linq;
using MapGeneration.TileSystem;
using UnityEditor;
using UnityEngine;

namespace MapGeneration
{
    public enum BrushTileType
    {
        Top, Bottom, Left, Right
    }

    /// <summary>
    /// This brush is used to place tiles in a chunks tiledata list
    /// </summary>
    [CreateAssetMenu(fileName = "ConnectionBrush", menuName = "MapGeneration/Brushes/ConnectionBrush")]
    [CustomGridBrush(false, true, false, "ConnectionBrush")]
    public class ConnectionBrush : GridBrush
    {
        //The current tiletype, when be used when a tile is placed
        public BrushTileType BrushTileType { get; set; }
        
        public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            //This tries to get a chunk component from the brush target
            Chunk chunk = brushTarget.GetComponent<Chunk>() ??
                          brushTarget.GetComponentInParent<Chunk>();
            
            //If a chunk was found, place a tile in the tiledata list
            if (chunk)
            {
                //If a chunk in the tiledata list allready this position, replace it else create new
                TileFlag connection = chunk.Connections.FirstOrDefault(x => x.Position == position);
                TileType tileType = TileType.Top;

                switch (BrushTileType)
                {
                    case BrushTileType.Top:
                        tileType = TileType.Top;
                        chunk.ChunkOpenings.TopOpen = true;
                        break;
                    case BrushTileType.Bottom:
                        tileType = TileType.Bottom;
                        chunk.ChunkOpenings.BottomOpen = true;
                        break;
                    case BrushTileType.Left:
                        tileType = TileType.Left;
                        chunk.ChunkOpenings.LeftOpen = true;
                        break;
                    case BrushTileType.Right:
                        tileType = TileType.Right;
                        chunk.ChunkOpenings.RightOpen = true;
                        break;
                }

                if (connection != null)
                {
                    connection.Type = tileType;
                    connection.Chunk = chunk;
                }
                else
                    chunk.Connections.Add(new TileFlag(position, tileType, chunk));
            }
            base.Paint(gridLayout,brushTarget,position);
        }


        public override void Erase(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            //This tries to get a chunk component from the brush target
            Chunk chunk = brushTarget.GetComponent<Chunk>() ??
                          brushTarget.GetComponentInParent<Chunk>();
           
            //If a chunk is found, erase a tile in the postion
            if (chunk)
            {
                TileFlag tileFlag = chunk.Connections.FirstOrDefault(x => x.Position == position);
                if (tileFlag != null)
                {
                    TileType type = tileFlag.Type;
                    chunk.Connections.Remove(tileFlag);
                    if (!chunk.Connections.Exists(x => x.Type == type))
                    {
                        switch (type)
                        {
                            case TileType.Top:
                                chunk.ChunkOpenings.TopOpen = false;
                                break;
                            case TileType.Bottom:
                                chunk.ChunkOpenings.BottomOpen = false;
                                break;
                            case TileType.Left:
                                chunk.ChunkOpenings.LeftOpen = false;
                                break;
                            case TileType.Right:
                                chunk.ChunkOpenings.RightOpen = false;
                                break;
                        }
                    }

                }
            }
            base.Erase(gridLayout,brushTarget,position);

        }
    }

}


