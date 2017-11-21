using System;
using System.Collections;
using System.Collections.Generic;
using MapGeneration.Algorithm;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MapGeneration
{
    [CreateAssetMenu(fileName = "Random Object Placer", menuName = "MapGeneration/Algorithms/Random Object Placer")]
    public class RandomObjectPlacerAlgorithm : MapGenerationAlgorithm
    {

        public override void PostProcess(Map map, List<Chunk> usableChunks)
        {
            //This goes throw all of the map's chunks
            for (int x = 0; x < map.Grid.GetLength(0); x++)
            {
                for (int y = 0; y < map.Grid.GetLength(1); y++)
                {
                    //If the chunk isn't instantiaded then skip it
                    if (!map.Grid[x, y].Instance)
                        continue;

                    List<MapObject> objects = new List<MapObject>();
                    ObjectType objectType = ObjectType.FlyingSpawner;

                    //This goes through all of the chunks tiles and removes the used connection tiles
                    foreach (var c in map.Grid[x, y].Instance.TileFlags)
                    {
                        bool instantiateObject = true;
                        switch (c.Type)
                        {
                            case TileType.Trap:
                                objectType = ObjectType.Trap;
                                break;
                            case TileType.Treasure:
                                objectType = ObjectType.Treasure;
                                break;
                            case TileType.FlyingSpawn:
                                objectType = ObjectType.FlyingSpawner;
                                break;
                            case TileType.GroundSpawn:
                                objectType = ObjectType.GroundSpawner;
                                break;
                            default:
                                instantiateObject = false;
                                break;
                        }
                        InstantiateRandomObect(ref objects,objectType, map,
                            map.Grid[x, y].Instance.Enviorment.GetCellCenterWorld(c.Position));
                    }

                }
            }
    }

        private void InstantiateRandomObect(ref List<MapObject> list, ObjectType type, Map map, Vector3 position )
        {
            list.Clear();
            foreach (var o in ResourceHandler.Instance.Objects)
            {
                if(o.Type == type)
                    list.Add(o);
            }
            if (list.Count > 0)
                Instantiate(list[Random.Range(0, list.Count)].gameObject, position, Quaternion.identity,map.transform);
            list.Clear();
        }
}
}

