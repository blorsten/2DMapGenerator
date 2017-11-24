using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MapGeneration
{
    public enum BrushTileFlag
    {
       Treasure, Trap, GroundSpawn, FlyingSpawn 
    }

    [CreateAssetMenu(fileName = "TileFlagBrush", menuName = "MapGeneration/Brushes/TileFlagBrush")]
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
                TileType tileType = TileType.Top;

                switch (BrushTileFlag)
                {
                    case BrushTileFlag.Treasure:
                        tileType = TileType.Treasure;
                        break;
                    case BrushTileFlag.Trap:
                        tileType = TileType.Trap;
                        break;
                    case BrushTileFlag.GroundSpawn:
                        tileType = TileType.GroundSpawn;
                        break;
                    case BrushTileFlag.FlyingSpawn:
                        tileType = TileType.FlyingSpawn;
                        break;
                }
                if (tileFlags != null)
                {
                    tileFlags.Type = tileType;
                    tileFlags.Chunk = chunk;
                }
                else
                    chunk.TileFlags.Add(new TileFlag(position,tileType,chunk));
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


