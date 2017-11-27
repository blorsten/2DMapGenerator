using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using MapGeneration.Extensions;
using MapGeneration.TileSystem;
using MapGeneration.Utils;


namespace MapGeneration
{
    public enum ChunkType
    {
        Default, DeadEnd, Reward, Secret, Start, End
    }

    /// <summary>
    /// Purpose:
    /// To Store the chunk's data.
    /// Creator:
    /// Mikkel Nielsen
    /// </summary>
    [ExecuteInEditMode]
    public class Chunk : MonoBehaviour
    {
        private const string TRAP_ICON_PATH = "Trap.png";
        private const string TREASURE_ICON_PATH = "Treasure.png";
        private const string GROUND_ICON_PATH = "Ground.png";
        private const string FLYING_ICON_PATH = "Flying.png";

        //This sections is for generel properties 
        [Header("Properties"), SerializeField]
        private int _width;//Tells the width of the chunk

        //Tells the height of the chunk
        [SerializeField] private int _height;
        
        //Tells the type of the chunk
        [SerializeField] private ChunkType _chunkType;

        [SerializeField] private bool _usedByConditionalChunk;

        //These fields tells what openings are open on the chunk
        [Header("Openings"), SerializeField] private ChunkOpenings _chunkOpenings;

        //This is a list of TileFlags in the chunk
        [SerializeField, ReadOnly] private List<TileFlag> _connections = new List<TileFlag>();

        [SerializeField, ReadOnly] private List<TileFlag> _tileTileFlags = new List<TileFlag>();

        //This section is for refernces
        [Header("Refernces"), SerializeField] private ChunkBehavior _chunkBehavior;
        [SerializeField] private Tilemap _enviorment;
        private ChunkHolder _chunkHolder;

        [Header("Draw Booleans"), SerializeField] private bool _drawConnections = true;
        [SerializeField] private bool _drawEdges = true;
        [SerializeField] private bool _drawTileFlags = true;

        //Properties for generel properties
        public int Width{ get { return _width; } set { _width = value; }}
        public int Height{get { return _height; } set { _height = value; }}
        public ChunkType ChunkType{get { return _chunkType; } set { _chunkType = value; }}

        public ChunkHolder ChunkHolder
        {
            get { return _chunkHolder; }
            set { _chunkHolder = value; }
        }

        //Properties for references
        public ChunkBehavior ChunkBehavior
        {
            get
            {
                if (!_chunkBehavior)
                    _chunkBehavior = GetComponent<ChunkBehavior>();
                return _chunkBehavior;
            }
            set
            {
                _chunkBehavior = value;
            }
        }

        public Tilemap Enviorment { get { return _enviorment; } set { _enviorment = value; } }

        public string ID { get; set; }//A ID to indentify the Chunk

        //A list for the items in the chunk
        public List<GameObject> Items { get; set; }

        public List<TileFlag> Connections{get { return _connections; } set { _connections = value; }}
        public List<TileFlag> TileFlags{get { return _tileTileFlags; }set { _tileTileFlags = value; }}

        public ChunkOpenings ChunkOpenings
        {
            get
            {
                return _chunkOpenings;
            }

            set
            {
                _chunkOpenings = value;
            }
        }

        public bool UsedByConditionalChunk
        {
            get { return _usedByConditionalChunk; }
        }

        public void OnDrawGizmos()
        {
            //This draws connections
            if (_drawConnections && Enviorment)
            {
                UnityEngine.Gizmos.color = new Color(231f / 255f, 76f / 255f, 60f / 255f);

                foreach (var c in Connections)
                {
                    Vector3 cellPosition = Enviorment.GetCellCenterWorld(c.Position);
                    Vector2 cellSize = Enviorment.cellSize;

                    Vector3 top = new Vector3(cellPosition.x, cellPosition.y + cellSize.y / 2);
                    Vector3 bottom = new Vector3(cellPosition.x, cellPosition.y - cellSize.y / 2);
                    Vector3 right = new Vector3(cellPosition.x + cellSize.x / 2, cellPosition.y);
                    Vector3 left = new Vector3(cellPosition.x - cellSize.x / 2, cellPosition.y);

                    switch (c.Type)
                    {
                        case TileType.Top:
                            GizmoUtilities.DrawArrow(top, ArrowDirection.Up);
                            break;
                        case TileType.Bottom:
                            GizmoUtilities.DrawArrow(bottom, ArrowDirection.Down);
                            break;
                        case TileType.Left:
                            GizmoUtilities.DrawArrow(left, ArrowDirection.Left);
                            break;
                        case TileType.Right:
                            GizmoUtilities.DrawArrow(right, ArrowDirection.Right);
                            break;
                    }
                }
            }

            //This draws tile flags
            if (_drawTileFlags && Enviorment)
            {
                foreach (var flag in TileFlags)
                {
                    Vector3 postition = Enviorment.GetCellCenterWorld(flag.Position);

                    switch (flag.Type)
                    {
                        case TileType.Trap:
                            UnityEngine.Gizmos.DrawIcon(postition, TRAP_ICON_PATH, true);
                            break;
                        case TileType.Treasure:
                            UnityEngine.Gizmos.DrawIcon(postition, TREASURE_ICON_PATH, true);
                            break;
                        case TileType.FlyingSpawn:
                            UnityEngine.Gizmos.DrawIcon(postition, FLYING_ICON_PATH, true);
                            break;
                        case TileType.GroundSpawn:
                            UnityEngine.Gizmos.DrawIcon(postition, GROUND_ICON_PATH, true);
                            break;

                    }
                }
            }
            //This draws edges
            if (_drawEdges && Enviorment)
            {
                UnityEngine.Gizmos.color = Color.white;
                Vector2 gridSize = new Vector2(Width, Height);
                Vector2 cellSize = Enviorment.cellSize;

                float yMin = transform.position.y - gridSize.y * cellSize.y / 2;
                float yMax = transform.position.y + gridSize.y * cellSize.y / 2;
                float xMin = transform.position.x - gridSize.x * cellSize.x / 2;
                float xMax = transform.position.x + gridSize.x * cellSize.x / 2;

                UnityEngine.Gizmos.DrawLine(new Vector3(xMin, yMin), new Vector3(xMin, yMax));
                UnityEngine.Gizmos.DrawLine(new Vector3(xMax, yMin), new Vector3(xMax, yMax));
                UnityEngine.Gizmos.DrawLine(new Vector3(xMin, yMin), new Vector3(xMax, yMin));
                UnityEngine.Gizmos.DrawLine(new Vector3(xMin, yMax), new Vector3(xMax, yMax));
            }

        }
    }
}
