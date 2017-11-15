using System.Collections;
using System.Collections.Generic;
using MapGeneration.Algorithm;
using UnityEditor;
using UnityEngine;

namespace MapGeneration.Algorithm
{
    [CreateAssetMenu(fileName = "Filler Algorithm", menuName = "MapGeneration/Algorithms/Filler Algorithm")]
    public class FillerAlgorithm : MapGenerationAlgorithm
    {
        public override void PostProcess(Map map, List<Chunk> usableChunks)
        {
            for (int x = 0; x < map.Grid.GetLength(0); x++)
            {
                for (int y = 0; y < map.Grid.GetLength(1); y++)
                {
                    if (!map.Grid[x, y].Instance)
                        continue;
                    foreach (var c in map.Grid[x, y].Instance.Connections)
                    {
                        switch (c.Type)
                        {
                            case ConnectionType.Top:
                                if (c.Chunk.ChunkHolder.ChunkOpenings.TopConnection)
                                    c.Chunk.Enviorment.SetTile(c.Position, null);
                                break;
                            case ConnectionType.Bottom:
                                if (c.Chunk.ChunkHolder.ChunkOpenings.BottomConnetion)
                                    c.Chunk.Enviorment.SetTile(c.Position, null);
                                break;
                            case ConnectionType.Left:
                                if (c.Chunk.ChunkHolder.ChunkOpenings.LeftConnection)
                                    c.Chunk.Enviorment.SetTile(c.Position, null);
                                break;
                            case ConnectionType.Right:
                                if (c.Chunk.ChunkHolder.ChunkOpenings.RightConnection)
                                    c.Chunk.Enviorment.SetTile(c.Position, null);
                                break;
                        }
                    }

                }
            }
        }
    }
}


