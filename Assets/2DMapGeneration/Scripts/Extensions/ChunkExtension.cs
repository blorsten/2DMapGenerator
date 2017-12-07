using MapGeneration.ChunkSystem;
using UnityEngine;

namespace MapGeneration.Extensions
{
    /// <summary>
    /// Purpose: Extension class used for utility method for chunks.
    /// Creator: MP
    /// </summary>
    public static class ChunkExtension
    {
        /// <summary>
        /// Compares a chunks size with another chunks size.
        /// </summary>
        /// <returns>Return true if of equal size.</returns>
        public static bool CompareSize(this Chunk a, Chunk b)
        {
            return a.Width == b.Width && a.Height == b.Height;
        }

        /// <summary>
        /// Compares a chunks size with another chunks size.
        /// </summary>
        /// <returns>Return true if of equal size.</returns>
        public static bool CompareSize(this Chunk a, int chunkSizeX, int chunkSizeY)
        {
            return a.Width == chunkSizeX && a.Height == chunkSizeY;
        }

        /// <summary>
        /// Compares a chunks size with another chunks size.
        /// </summary>
        /// <returns>Return true if of equal size.</returns>
        public static bool CompareSize(this Chunk a, Vector2Int chunkSize)
        {
            return a.Width == chunkSize.x && a.Height == chunkSize.y;
        }
    }
}