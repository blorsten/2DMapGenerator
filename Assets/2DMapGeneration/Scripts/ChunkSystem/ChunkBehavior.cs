using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration.ChunkSystem
{
    /// <summary>
    /// behavior to the chunk
    /// </summary>
    public class ChunkBehavior : MonoBehaviour
    {
        [Header("The coresponding Chunk to this behavior"), SerializeField]
        private Chunk _chunk;

        [Header("Objects used to fill unused exits"), SerializeField]
        private List<GameObject> _exitFillers;

        public Chunk Chunk
        {
            get
            {
                if (!_chunk)
                    _chunk = GetComponent<Chunk>();

                return _chunk;
            }
            set { _chunk = value; }
        }

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