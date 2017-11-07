using System.Collections.Generic;
using UnityEngine;

namespace ChunkBehavior
{
    /// <summary>
    /// Purpose:
    /// To add behavior to the chunks, such as close exits or more.
    /// Creator:
    /// Niels Justesen
    /// </summary>
    public class ChunkBehavior : MonoBehaviour 
    {
        [SerializeField]
        private Chunk chunk;
        public Chunk Chunk { get { return chunk; } }

        private List<GameObject> exitFillers;

        private void Start()
        {
            
        }

        private void Update()
        {
            
        }

        private void CloseExits()
        {

        }

        public void UpdateChunk()
        {

        }

    }
}