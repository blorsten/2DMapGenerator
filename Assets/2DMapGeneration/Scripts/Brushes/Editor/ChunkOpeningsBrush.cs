using System.Linq;
using MapGeneration.ChunkSystem;
using MapGeneration.TileSystem;
using UnityEditor;
using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// This enum is used to determine what direction of the connection
    /// </summary>
    public enum ConnectionType
    {
        Top, Bottom, Left, Right
    }

    /// <summary>
    /// This brush is used to place chunk openings in chunks.
    /// </summary>
    [CustomGridBrush(false, true, false, "ChunkOpeningsBrush")]
    public class ChunkOpeningsBrush : GridBrush
    {
        /// <summary>
        /// The current tiletype, when be used when a tile is placed.
        /// </summary>
        public ConnectionType ConnectionType { get; set; }
        
        /// <summary>
        /// This is called when the brush is painting, it tries to get a chunk on the current 
        /// tilemap and if it does, then it places a chunk opening in the current position
        /// </summary>
        /// <param name="gridLayout"></param>
        /// <param name="brushTarget"></param>
        /// <param name="position"></param>
        public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            //This tries to get a chunk component from the brush target
            Chunk chunk = brushTarget.GetComponent<Chunk>() ??
                          brushTarget.GetComponentInParent<Chunk>();
            
            //If a chunk was found, place a tile in the tiledata list
            if (chunk)
            {
                //If a chunk in the tiledata list already this position, replace it else create new
                TileFlag connection = chunk.Openings.FirstOrDefault(x => x.Position == position);
                FlagType flagType = FlagType.Top;

                switch (ConnectionType)
                {
                    case ConnectionType.Top:
                        flagType = FlagType.Top;
                        chunk.ChunkOpenings.TopOpen = true;
                        break;
                    case ConnectionType.Bottom:
                        flagType = FlagType.Bottom;
                        chunk.ChunkOpenings.BottomOpen = true;
                        break;
                    case ConnectionType.Left:
                        flagType = FlagType.Left;
                        chunk.ChunkOpenings.LeftOpen = true;
                        break;
                    case ConnectionType.Right:
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
                    chunk.Openings.Add(new TileFlag(position, flagType, chunk));
            }
        }

        /// <summary>
        /// This is called when the brush is erasing, it tries to get a chunk on the current 
        /// tilemap and if it does, then it removes a chunk opening in the current position
        /// </summary>
        /// <param name="gridLayout"></param>
        /// <param name="brushTarget"></param>
        /// <param name="position"></param>
        public override void Erase(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            //This tries to get a chunk component from the brush target
            Chunk chunk = brushTarget.GetComponent<Chunk>() ??
                          brushTarget.GetComponentInParent<Chunk>();
           
            //If a chunk is found, erase a tile in the position
            if (chunk)
            {
                TileFlag tileFlag = chunk.Openings.FirstOrDefault(x => x.Position == position);
                if (tileFlag != null)
                {
                    FlagType type = tileFlag.Type;
                    chunk.Openings.Remove(tileFlag);
                    if (!chunk.Openings.Exists(x => x.Type == type))
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


