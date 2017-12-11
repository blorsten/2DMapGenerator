using System.Linq;
using MapGeneration.ChunkSystem;
using MapGeneration.TileSystem;
using UnityEditor;
using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// This enum is used to dertermine what kind of tileflag should be placed.
    /// </summary>
    public enum BrushTileFlag
    {
       Treasure, Trap, GroundSpawn, FlyingSpawn 
    }

    /// <summary>
    /// This brush is used to place tileflags other than openings.
    /// </summary>
    [CustomGridBrush(false, true, false, "TileFlagBrush")]
    public class TileFlagBrush : GridBrush
    {
        /// <summary>
        /// This enum is set in the brush inspector and used as the current tileflag.
        /// </summary>
        public BrushTileFlag BrushTileFlag { get; set; }

        /// <summary>
        /// This is called when the brush is painting, it tries to get a chunk from the current
        /// tilemap and if it does, then is places a tileflag on the current position
        /// </summary>
        /// <param name="gridLayout"></param>
        /// <param name="brushTarget"></param>
        /// <param name="position"></param>
        public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            //This tries to get a chunk component from the brush target
            Chunk chunk = brushTarget.GetComponent<Chunk>() ??
                          brushTarget.GetComponentInParent<Chunk>();

            if (chunk)
            {
                //If a chunk in the tiledata list allready this position, replace it else create new
                TileFlag tileFlags = chunk.TileFlags.FirstOrDefault(x => x.Position == position);
                FlagType flagType = FlagType.Top;

                switch (BrushTileFlag)
                {
                    case BrushTileFlag.Treasure:
                        flagType = FlagType.Treasure;
                        break;
                    case BrushTileFlag.Trap:
                        flagType = FlagType.Trap;
                        break;
                    case BrushTileFlag.GroundSpawn:
                        flagType = FlagType.GroundSpawn;
                        break;
                    case BrushTileFlag.FlyingSpawn:
                        flagType = FlagType.FlyingSpawn;
                        break;
                }
                if (tileFlags != null)
                {
                    tileFlags.Type = flagType;
                    tileFlags.Chunk = chunk;
                }
                else
                    chunk.TileFlags.Add(new TileFlag(position,flagType,chunk));
            }
        }

        /// <summary>
        /// This is called when the brush is erasing, it tries to get a chunk on the current 
        /// tilemap and if it does, then it removes a tile falg in the currect position
        /// </summary>
        /// <param name="gridLayout"></param>
        /// <param name="brushTarget"></param>
        /// <param name="position"></param>
        public override void Erase(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            //This tries to get a chunk component from the brush target
            Chunk chunk = brushTarget.GetComponent<Chunk>() ??
                          brushTarget.GetComponentInParent<Chunk>();

            //If a chunk is found, erase a tile in the postion
            if (chunk)
            {
                TileFlag tileFlag = chunk.TileFlags.FirstOrDefault(x => x.Position == position);
                if (tileFlag != null)
                    chunk.TileFlags.Remove(tileFlag);
            }


        }
    }

}


