using System;
using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration.Algorithm
{
    [Serializable]
    public struct AlgorithmStorage
    {
        [SerializeField] public bool IsActive;
        [SerializeField] public MapGenerationAlgorithm Algorithm;

        public AlgorithmStorage(MapGenerationAlgorithm algorithm)
        {
            Algorithm = algorithm;
            IsActive = true;
        }
    }
    /// <summary>
    /// Purpose:
    /// Creator:
    /// </summary>
    /// 

    public class MapGenerationAlgorithm : ScriptableObject
    {
        

        /// <summary>
        /// Process a given map
        /// </summary>
        /// <param name="map">map to process</param>
        /// <param name="usableChunks">List of all the usable chunks, filtered by <see cref="MapBlueprint.Generate"/>.</param>
        public virtual bool Process(Map map, List<Chunk> usableChunks)
        {
            return true;
        }

        /// <summary>
        /// Runs after the process has been run.
        /// </summary>
        /// <param name="map">map to process</param>
        /// <param name="usableChunks">List of all the usable chunks, filtered by <see cref="MapBlueprint.Generate"/>.</param>
        public virtual bool PostProcess(Map map, List<Chunk> usableChunks)
        {
            return true;
        }

        /// <summary>
        /// Takes a current position and checks out from a directions if its a valid move on the grid.
        /// </summary>
        /// <param name="currentPosition"></param>
        /// <param name="nextDir"></param>
        /// <param name="currentMap"></param>
        /// <returns></returns>
        protected Vector2Int? CheckNextPosition(Vector2Int currentPosition, PathAlgorithm.CardinalDirections nextDir, Map currentMap)
        {
            switch (nextDir)
            {
                case PathAlgorithm.CardinalDirections.Top:
                    currentPosition += new Vector2Int(0, 1);
                    if (currentPosition.y < currentMap.MapBlueprint.GridSize.y && currentPosition.y >= 0)
                    {
                        return currentPosition;
                    }
                    break;
                case PathAlgorithm.CardinalDirections.Bottom:
                    currentPosition += new Vector2Int(0, -1);
                    if (currentPosition.y < currentMap.MapBlueprint.GridSize.y && currentPosition.y >= 0)
                    {
                        return currentPosition;
                    }
                    break;
                case PathAlgorithm.CardinalDirections.Right:
                    currentPosition += new Vector2Int(1, 0);
                    if (currentPosition.x < currentMap.MapBlueprint.GridSize.x && currentPosition.x >= 0)
                    {
                        return currentPosition;
                    }
                    break;
                case PathAlgorithm.CardinalDirections.Left:
                    currentPosition += new Vector2Int(-1, 0);
                    if (currentPosition.x < currentMap.MapBlueprint.GridSize.x && currentPosition.x >= 0)
                    {
                        return currentPosition;
                    }
                    break;
            }

            return null;
        }

        /// <summary>
        /// Sets the connections in the queue of marked chunks
        /// </summary>
        /// <param name="dir">The direction the algorithm took</param>
        /// <param name="current">the current chunk</param>
        /// <param name="next">the next chunk in the queue</param>
        /// <param name="type">The desired type of connection</param>
        protected void SetChunkConnections(PathAlgorithm.CardinalDirections dir, ChunkHolder current, ChunkHolder next, ChunkOpenings.ConnectionType type = ChunkOpenings.ConnectionType.Default)
        {
            current.ChunkOpenings.SetConnectionAuto(dir, next, type);
        }

        /// <summary>
        /// Cleans all shared data in algorithm.
        /// </summary>
        protected virtual void Reset()
        {
            
        }
    }
}