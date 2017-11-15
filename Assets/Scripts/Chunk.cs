using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapGeneration
{
    public enum ChunkType
    {
        Default, DeadEnd, Reward, Secret
    }

    /// <summary>
    /// Purpose:
    /// To Store the chunk's data.
    /// Creator:
    /// Mikkel Nielsen
    /// </summary>
    public class Chunk : MonoBehaviour
    {
        //This sections is for generel properties 
        [Header("Properties"), SerializeField]
        private int _width;//Tells the width of the chunk

        //Tells the height of the chunk
        [SerializeField] private int _height;
        
        //Tells the type of the chunk
        [SerializeField] private ChunkType _chunkType;
        
        //This section if for refernces
        [Header("Refernces"), SerializeField] private ChunkBehavior _chunkBehavior;

        [SerializeField] private Tilemap _enviorment;
        [SerializeField] private ChunkHolder _chunkHolder;

        //Properties for generel stuff
        public int Width { get { return _width; } set { _width = value; } }
        public int Height { get { return _height; } set { _height = value; } }
        public ChunkType ChunkType { get { return _chunkType; } set { _chunkType = value; } }

        public ChunkHolder ChunkHolder
        {
            get { return _chunkHolder; }
            set { _chunkHolder = value; }
        }

        //Properties for references
        public ChunkBehavior ChunkBehavior { get { return _chunkBehavior; } set { _chunkBehavior = value; } }
        public Tilemap Enviorment { get { return _enviorment; } set { _enviorment = value; } }

        public string ID { get; set; }//A ID to indentify the Chunk

        //A list for the items in the chunk
        public List<GameObject> Items { get; set; }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (ChunkHolder.ChunkOpenings.TopConnection)            
                Gizmos.DrawLine(this.transform.position, transform.position + Vector3.up * (Height / 2f));
            if (ChunkHolder.ChunkOpenings.BottomConnetion)
                Gizmos.DrawLine(this.transform.position, transform.position + Vector3.down * (Height / 2f));
            if (ChunkHolder.ChunkOpenings.RightConnection)
                Gizmos.DrawLine(this.transform.position, transform.position + Vector3.right * (Width / 2f));
            if (ChunkHolder.ChunkOpenings.LeftConnection)
                Gizmos.DrawLine(this.transform.position, transform.position + Vector3.left * (Width / 2f));
        }

    }
}
