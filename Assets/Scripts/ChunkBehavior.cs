using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// Purpose:
    /// To add behavior to the chunks, such as close exits or more.
    /// Creator:
    /// Niels Justesen
    /// </summary>
    public class ChunkBehavior : MonoBehaviour 
    {
        [Header("The coresponding Chunk to this behavior"),SerializeField]
        private Chunk chunk;

        [Header("Objects used to fill unused exits"), SerializeField]
        private List<GameObject> exitFillers;

        public Chunk Chunk { get { return chunk; } }

        public virtual void Start()
        {
            
        }

        public virtual void Update()
        {
            
        }

        public virtual void CloseExits()
        {

        }

        public virtual void UpdateChunk()
        {

        }
    }
}