using System.Linq;
using MapGeneration.TileSystem;
using UnityEditor;
using UnityEngine;

namespace MapGeneration
{
    public enum BrushTileFlag
    {
       Treasure, Trap, GroundSpawn, FlyingSpawn 
    }

    [CustomGridBrush(false, true, false, "TileFlagBrush")]
    public class TileFlagBrush : GridBrush
    {

        public BrushTileFlag BrushTileFlag { get; set; }

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


