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
                FlagType flagType = FlagType.Top;

                switch (BrushTileType)
                {
                    case BrushTileType.Top:
                        flagType = FlagType.Top;
                        chunk.ChunkOpenings.TopOpen = true;
                        break;
                    case BrushTileType.Bottom:
                        flagType = FlagType.Bottom;
                        chunk.ChunkOpenings.BottomOpen = true;
                        break;
                    case BrushTileType.Left:
                        flagType = FlagType.Left;
                        chunk.ChunkOpenings.LeftOpen = true;
                        break;
                    case BrushTileType.Right:
                        flagType = FlagType.Right;
                        chunk.ChunkOpenings.RightOpen = true;
                        break;
                }

                if (connection != null)
                {
                    connection.Type = flagType;
                    connection.Chunk = chunk;
                }
                else
                    chunk.Connections.Add(new TileFlag(position, flagType, chunk));
            }
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
                    FlagType type = tileFlag.Type;
                    chunk.Connections.Remove(tileFlag);
                    if (!chunk.Connections.Exists(x => x.Type == type))
                    {
                        switch (type)
                        {
                            case FlagType.Top:
                                chunk.ChunkOpenings.TopOpen = false;
                                break;
                            case FlagType.Bottom:
                                chunk.ChunkOpenings.BottomOpen = false;
                                break;
                            case FlagType.Left:
                                chunk.ChunkOpenings.LeftOpen = false;
                                break;
                            case FlagType.Right:
                                chunk.ChunkOpenings.RightOpen = false;
                                break;
                        }
                    }

                }
            }

        }
    }

}


