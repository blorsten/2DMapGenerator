using MapGeneration.ChunkSystem;
using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// Root class for all gameplay objects.
    /// </summary>
    public class GameplayObject : MonoBehaviour
    {
        //Reference to the chunk that has this gameplay object.
        public Chunk Owner { get; set; }
    }
}


