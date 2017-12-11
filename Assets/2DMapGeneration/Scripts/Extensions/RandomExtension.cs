using UnityEngine;

namespace MapGeneration.Extensions
{
    /// <summary>
    /// An extension for unity random class.
    /// </summary>
    public static class RandomExtension
    {
        /// <summary>
        /// Fills up a byte array with randomized bytes.
        /// </summary>
        /// <param name="byteArray">Target byte array.</param>
        /// <returns>A byte array filled with randomized bytes.</returns>
        public static byte[] NextBytes(this byte[] byteArray)
        {
            for (var i = 0; i < byteArray.Length; i++)
            {
                byteArray[i] = (byte) Random.Range(0, 256);
            }

            return byteArray;
        }

        /// <summary>
        /// Creates a byte array with a length of 16 bytes, filled with randomized bytes.
        /// Used for <see cref="System.Guid"/>.
        /// </summary>
        /// <returns>Randomized byte array.</returns>
        public static byte[] GenerateByteSeed()
        {
            return new byte[16].NextBytes();
        }

        /// <summary>
        /// Creates a byte array with a length of 16 bytes, filled with randomized bytes using a predefined Random.
        /// </summary>
        /// <param name="random">The predefined random.</param>
        /// <returns>Randomized byte array.</returns>
        public static byte[] GenerateByteSeed(System.Random random)
        {
            byte[] bytes = new byte[16];
            random.NextBytes(bytes);
            return bytes;
        }

        /// <summary>
        /// Returns a random integer number between min [inclusive] and max [exclusive].
        /// </summary>
        /// <param name="random"></param>
        /// <param name="min">Min integer [inclusive].</param>
        /// <param name="max">Max int [exclusive].</param>
        /// <returns>Randomized value between min and max.</returns>
        public static int Range(this System.Random random, int min, int max)
        {
            return random.Next(min, max);
        }

        /// <summary>
        /// Returns a random integer number between min [inclusive] and max [inclusive].
        /// </summary>
        /// <param name="random"></param>
        /// <param name="min">Min float [inclusive].</param>
        /// <param name="max">Max float [inclusive].</param>
        /// <returns>Randomized value between min and max.</returns>
        public static float Range(this System.Random random, float min, float max)
        {
            return (float) (random.NextDouble() * (max - min) + min);
        }

        /// <summary>
        /// Returns a randomized Vector2 with its components randomized between min [inclusive] and max [inclusive].
        /// </summary>
        /// <param name="random"></param>
        /// <param name="min">Vector2 with min component values [inclusive].</param>
        /// <param name="max">Vector2 with max component values [inclusive].</param>
        /// <returns>Vector2 with randomized components between min and max.</returns>
        public static Vector2 Range(this System.Random random, Vector2 min, Vector2 max)
        {
            return new Vector2(random.Range(min.x, max.x), random.Range(min.y, max.y));
        }

        /// <summary>
        /// Returns a randomized Vector2Int with its components randomized between min [inclusive] and max [exclusive].
        /// </summary>
        /// <param name="random"></param>
        /// <param name="min">Vector2Int with min component values [inclusive].</param>
        /// <param name="max">Vector2Int with min component values [exclusive].</param>
        /// <returns>Vector2Int with randomized components between min and max.</returns>
        public static Vector2Int Range(this System.Random random, Vector2Int min, Vector2Int max)
        {
            return new Vector2Int(random.Range(min.x, max.x), random.Range(min.y, max.y));
        }
    }
}