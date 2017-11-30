 ﻿using System.Collections.Generic;
using MapGeneration.ConditionalChunks;
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
    [SelectionBase]
    public class Chunk : MonoBehaviour
    {
        private const string TRAP_ICON_PATH = "Trap.png";
        private const string TREASURE_ICON_PATH = "Treasure.png";
        private const string GROUND_ICON_PATH = "Ground.png";
        private const string FLYING_ICON_PATH = "Flying.png";

        private ConditionalChunk _conditionalChunk;

        //This sections is for generel properties 
        [Header("Properties")]
        //Tells the width of the chunk
        [SerializeField] private int _width;

        //Tells the height of the chunk
        [SerializeField] private int _height;

        //Tells the type of the chunk
        [SerializeField] private ChunkType _chunkType;

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

        [SerializeField, Tooltip("Is this chunk dependendant on some other chunk?")]
        private bool _isStandaloneChunk = true;

        //Properties for generel properties
        public int Width { get { return _width; } set { _width = value; } }
        public int Height { get { return _height; } set { _height = value; } }
        public ChunkType ChunkType { get { return _chunkType; } set { _chunkType = value; } }
        private float[,] biomeValues;
        public float[,] BiomeValues { get { return biomeValues; } set { biomeValues = value; } }

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

        public List<TileFlag> Connections{ get { return _connections; } set { _connections = value; } }
        public List<TileFlag> TileFlags{ get { return _tileTileFlags; } set { _tileTileFlags = value; } }

        //Lazy loading properties
        public ConditionalChunk ConditionalChunk { get { return _conditionalChunk ?? (_conditionalChunk = GetComponent<ConditionalChunk>()); } }

        public ChunkOpenings ChunkOpenings { get { return _chunkOpenings; } set { _chunkOpenings = value; } }

        public bool IsStandaloneChunk { get { return _isStandaloneChunk; } set { _isStandaloneChunk = value; } }

		public Map Map { get; set; }

        /// <summary>
        /// When this components gets added or gets reset, grabs om references if it can.
        /// </summary>
        void Reset()
        {
            _enviorment = GetComponentInChildren<Tilemap>();

            _chunkBehavior = GetComponent<ChunkBehavior>();
            if (!_chunkBehavior)
                _chunkBehavior = GetComponentInChildren<ChunkBehavior>();

            if (_chunkBehavior)
                _chunkBehavior.Chunk = this;

        }

        private void Awake()
        {
            biomeValues = new float[_width, _height];
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
                        case FlagType.Top:
                            GizmoUtilities.DrawArrow(top, ArrowDirection.Up);
                            break;
                        case FlagType.Bottom:
                            GizmoUtilities.DrawArrow(bottom, ArrowDirection.Down);
                            break;
                        case FlagType.Left:
                            GizmoUtilities.DrawArrow(left, ArrowDirection.Left);
                            break;
                        case FlagType.Right:
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
                        case FlagType.Trap:
                            UnityEngine.Gizmos.DrawIcon(postition, TRAP_ICON_PATH, true);
                            break;
                        case FlagType.Treasure:
                            UnityEngine.Gizmos.DrawIcon(postition, TREASURE_ICON_PATH, true);
                            break;
                        case FlagType.FlyingSpawn:
                            UnityEngine.Gizmos.DrawIcon(postition, FLYING_ICON_PATH, true);
                            break;
                        case FlagType.GroundSpawn:
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

        public void RefreshTilemaps()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Tilemap tilemap = transform.GetChild(i).gameObject.GetComponent<Tilemap>();
                if(tilemap != null)
                    tilemap.RefreshAllTiles();
            }
        }

    }
}
