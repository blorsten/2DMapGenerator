using System.Collections.Generic;
using MapGeneration.ChunkSystem;
using MapGeneration.TileSystem;

namespace MapGeneration.Algorithm
{
    /// <summary>
    /// This algoritm find the used connections in chunks a removes the tiles in that connection.
    /// </summary>
    public class ConnectionOpenerAlgorithm : MapGenerationAlgorithm
    {
        public override bool PostProcess(Map map, List<Chunk> usableChunks)
        {
            //This goes throw all of the map's chunks
            for (int x = 0; x < map.Grid.GetLength(0); x++)
            {
                for (int y = 0; y < map.Grid.GetLength(1); y++)
                {
                    //If the chunk isn't instantiaded then skip it
                    if (!map.Grid[x, y].Instance)
                        continue;

                    //This goes through all of the chunks tiles and removes the used connection tiles
                    foreach (var c in map.Grid[x, y].Instance.Connections)
                    {
                        switch (c.Type)
                        {
                            case FlagType.Top:
                                if (c.Chunk.ChunkHolder.ChunkOpenings.TopConnection)
                                    c.Chunk.Enviorment.SetTile(c.Position, null);
                                break;
                            case FlagType.Bottom:
                                if (c.Chunk.ChunkHolder.ChunkOpenings.BottomConnetion)
                                    c.Chunk.Enviorment.SetTile(c.Position, null);
                                break;
                            case FlagType.Left:
                                if (c.Chunk.ChunkHolder.ChunkOpenings.LeftConnection)
                                    c.Chunk.Enviorment.SetTile(c.Position, null);
                                break;
                            case FlagType.Right:
                                if (c.Chunk.ChunkHolder.ChunkOpenings.RightConnection)
                                    c.Chunk.Enviorment.SetTile(c.Position, null);
                                break;
                        }
                    }

                }
            }

            return true;
        }
    }
}


