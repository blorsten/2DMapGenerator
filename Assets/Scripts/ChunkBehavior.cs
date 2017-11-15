using System;
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
        }

        public virtual void Start()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void CloseExits()
        {
            foreach (var c in Chunk.Connections){
                switch (c.Type)
                {
                    case ConnectionType.Top:
                        if(Chunk.ChunkHolder.ChunkOpenings.TopConnection)
                            Chunk.Enviorment.SetTile(c.Position,null);
                        break;
                    case ConnectionType.Bottom:
                        if (Chunk.ChunkHolder.ChunkOpenings.BottomConnetion)
                            Chunk.Enviorment.SetTile(c.Position, null);
                        break;
                    case ConnectionType.Left:
                        if (Chunk.ChunkHolder.ChunkOpenings.LeftConnection)
                            Chunk.Enviorment.SetTile(c.Position, null);
                        break;
                    case ConnectionType.Right:
                        if (Chunk.ChunkHolder.ChunkOpenings.RightConnection)
                            Chunk.Enviorment.SetTile(c.Position, null);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public virtual void UpdateChunk()
        {

        }
    }
}