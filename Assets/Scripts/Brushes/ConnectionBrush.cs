using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                        break;
                    case BrushTileType.Bottom:
                        tileType = TileType.Bottom;
                        break;
                    case BrushTileType.Left:
                        tileType = TileType.Left;
                        break;
                    case BrushTileType.Right:
                        tileType = TileType.Right;
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
                    chunk.Connections.Remove(tileFlag);
            }


        }
    }

}


