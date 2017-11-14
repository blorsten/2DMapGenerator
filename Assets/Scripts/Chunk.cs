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

        //These fields tells what openings are open on the chunk
        [Header("Openings"), SerializeField] private bool _topOpen;

        [SerializeField] private bool _bottomOpen;
        [SerializeField] private bool _leftOpen;
        [SerializeField] private bool _rightOpen;

        //This section if for refernces
        [Header("Refernces"), SerializeField] private ChunkBehavior _chunkBehavior;

        [SerializeField] private Tilemap _enviorment;

        [SerializeField] private List<Vector3Int> _topConnections = new List<Vector3Int>();
        [SerializeField, HideInInspector] private List<Vector3Int> _bottomConnections = new List<Vector3Int>();
        [SerializeField, HideInInspector] private List<Vector3Int> _leftConnections = new List<Vector3Int>();
        [SerializeField, HideInInspector] private List<Vector3Int> _rightConnections = new List<Vector3Int>();


        //Properties for generel properties
        public int Width{ get { return _width; } set { _width = value; }}
        public int Height{get { return _height; } set { _height = value; }}
        public ChunkType ChunkType{get { return _chunkType; } set { _chunkType = value; }}

        //Properties for opennings
        public bool TopOpen{get { return _topOpen; } set { _topOpen = value; }}
        public bool BottomOpen{get { return _bottomOpen; } set { _bottomOpen = value; }}
        public bool LeftOpen{get { return _leftOpen; } set { _leftOpen = value; }}
        public bool RightOpen{get { return _rightOpen; } set { _rightOpen = value; }}

        //Properties for references
        public ChunkBehavior ChunkBehavior{get { return _chunkBehavior; } set { _chunkBehavior = value; }}
        public Tilemap Enviorment{get { return _enviorment; } set { _enviorment = value; }}

        public string ID { get; set; }//A ID to indentify the Chunk

        //These properties tells te chunk whick openings are used
        public bool TopConnection { get; set; }
        public bool BottomConnetion { get; set; }
        public bool LeftConnection { get; set; }
        public bool RightConnection { get; set; }

        //A list for the items in the chunk
        public List<GameObject> Items { get; set; }

        public List<Vector3Int> TopConnections{get { return _topConnections; }set { _topConnections = value; }}
        public List<Vector3Int> BottomConnections{get { return _bottomConnections; }set { _bottomConnections = value; }}
        public List<Vector3Int> LeftConnections{get { return _leftConnections; }set { _leftConnections = value; }}
        public List<Vector3Int> RightConnections{get { return _rightConnections; }set { _rightConnections = value; }}

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (TopConnection)            
                Gizmos.DrawLine(this.transform.position, transform.position + Vector3.up * (Height / 2f));
            if (BottomConnetion)
                Gizmos.DrawLine(this.transform.position, transform.position + Vector3.down * (Height / 2f));
            if (RightConnection)
                Gizmos.DrawLine(this.transform.position, transform.position + Vector3.right * (Width / 2f));
            if (LeftConnection)
                Gizmos.DrawLine(this.transform.position, transform.position + Vector3.left * (Width / 2f));

            Gizmos.color = Color.blue;
            foreach (var c in TopConnections)
            {
                Gizmos.DrawCube(Enviorment.GetCellCenterWorld(c),Enviorment.cellSize/4);
            }

            Gizmos.color = Color.green;
            foreach (var c in BottomConnections)
            {
                Gizmos.DrawCube(Enviorment.GetCellCenterWorld(c), Enviorment.cellSize / 4);
            }

            Gizmos.color = Color.red;
            foreach (var c in RightConnections)
            {
                Gizmos.DrawCube(Enviorment.GetCellCenterWorld(c), Enviorment.cellSize / 4);
            }

            Gizmos.color = Color.magenta;
            foreach (var c in LeftConnections)
            {
                Gizmos.DrawCube(Enviorment.GetCellCenterWorld(c), Enviorment.cellSize / 4);
            }

        }
    }
}
