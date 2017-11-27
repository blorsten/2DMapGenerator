using System;
using System.Collections;
using System.Collections.Generic;
using MapGeneration.Algorithm;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MapGeneration
{
    [CreateAssetMenu(fileName = "New Random Object Placer", menuName = "MapGeneration/Algorithms/Random Object Placer")]
    public class RandomObjectPlacerAlgorithm : MapGenerationAlgorithm
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

                    List<GameplayObject> objects = new List<GameplayObject>();

                    //This goes through all of the chunks tiles and removes the used connection tiles
                    foreach (var c in map.Grid[x, y].Instance.TileFlags)
                    {
                        var position = map.Grid[x, y].Instance.Enviorment
                            .GetCellCenterWorld(c.Position);
                        Chunk chunk = map.Grid[x, y].Instance;

                        switch (c.Type)
                        {
                            case FlagType.Trap:
                                InstantiateRandomObject<Trap>(ref objects, chunk, position);
                                break;
                            case FlagType.Treasure:
                                InstantiateRandomObject<Treasure>(ref objects, chunk, position);
                                break;
                        }

                    }

                }
            }

            return true;
        }

        private void InstantiateRandomObject<T>(ref List<GameplayObject> list, Chunk chunk,
            Vector3 position) where T : GameplayObject
        {
            list.Clear();
            foreach (var o in ResourceHandler.Instance.Objects)
            {
                if(o.GetType() == typeof(T))
                    list.Add(o);
            }
            if (list.Count > 0)
                Instantiate(list[Random.Range(0, list.Count)].gameObject, position, Quaternion.identity, chunk.transform);
            list.Clear();
        }
}
}

