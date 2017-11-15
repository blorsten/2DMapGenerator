using System;
using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration.Algorithm
{
    /// <summary>
    /// Purpose:
    /// Creator:
    /// </summary>
    public class MapGenerationAlgorithm : ScriptableObject
    {
        /// <summary>
        /// Process a given map
        /// </summary>
        /// <param name="map">map to process</param>
        /// <param name="usableChunks">List of all the usable chunks, filtered by <see cref="MapBlueprint.Generate"/>.</param>
        public virtual void Process(Map map, List<Chunk> usableChunks)
        {

        }

        /// <summary>
        /// Runs after the process has been run.
        /// </summary>
        /// <param name="map">map to process</param>
        /// <param name="usableChunks">List of all the usable chunks, filtered by <see cref="MapBlueprint.Generate"/>.</param>
        public virtual void PostProcess(Map map, List<Chunk> usableChunks)
        {

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
        protected void SetChunkConnections(PathAlgorithm.CardinalDirections dir, ChunkHolder current, ChunkHolder next)
        {
            switch (dir)
            {
                case PathAlgorithm.CardinalDirections.Top:
                    current.Instance.ChunkHolder.ChunkOpenings.TopConnection = true;
                    next.Instance.ChunkHolder.ChunkOpenings.BottomConnetion = true;
                    break;
                case PathAlgorithm.CardinalDirections.Bottom:
                    current.Instance.ChunkHolder.ChunkOpenings.BottomConnetion = true;
                    next.Instance.ChunkHolder.ChunkOpenings.TopConnection = true;
                    break;
                case PathAlgorithm.CardinalDirections.Left:
                    current.Instance.ChunkHolder.ChunkOpenings.LeftConnection = true;
                    next.Instance.ChunkHolder.ChunkOpenings.RightConnection = true;
                    break;
                case PathAlgorithm.CardinalDirections.Right:
                    current.Instance.ChunkHolder.ChunkOpenings.RightConnection = true;
                    next.Instance.ChunkHolder.ChunkOpenings.LeftConnection = true;
                    break;
            }
        }

        /// <summary>
        /// Cleans all shared data in algorithm.
        /// </summary>
        protected virtual void Reset()
        {
            
        }
    }
}