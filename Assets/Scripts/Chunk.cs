using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using MapGeneration.Extensions;

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

        [SerializeField] private List<Connection> _connections = new List<Connection>();

        //This section if for refernces
        [Header("Refernces"), SerializeField] private ChunkBehavior _chunkBehavior;

        [SerializeField] private Tilemap _enviorment;
        [SerializeField] private ChunkHolder _chunkHolder;

        [Header("Gizmos"), SerializeField] private bool _showConnectionWhenPlay = true;
        [SerializeField] private bool _showBacktrackingWhenPlay = true;
        [SerializeField] private bool _showEdgesWhenPlay = true;

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

        public List<Connection> Connections{get { return _connections; } set { _connections = value; }}


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (!(!_showBacktrackingWhenPlay && Application.isPlaying))
            {
                if (ChunkHolder.ChunkOpenings.TopConnection)
                    Gizmos.DrawLine(this.transform.position,
                        transform.position + Vector3.up * (Height / 2f));
                if (ChunkHolder.ChunkOpenings.BottomConnetion)
                    Gizmos.DrawLine(this.transform.position,
                        transform.position + Vector3.down * (Height / 2f));
                if (ChunkHolder.ChunkOpenings.RightConnection)
                    Gizmos.DrawLine(this.transform.position,
                        transform.position + Vector3.right * (Width / 2f));
                if (ChunkHolder.ChunkOpenings.LeftConnection)
                    Gizmos.DrawLine(this.transform.position,
                        transform.position + Vector3.left * (Width / 2f));
            }

            if (!(!_showConnectionWhenPlay && Application.isPlaying) && Enviorment)
            {
                Gizmos.color = new Color(231f / 255f, 76f / 255f, 60f / 255f);
                 
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
                        case ConnectionType.Top:
                            GizmoUtilities.DrawArrow(top,ArrowDirection.Up);
                            break;
                        case ConnectionType.Bottom:
                            GizmoUtilities.DrawArrow(bottom, ArrowDirection.Down);
                            break;
                        case ConnectionType.Left:
                            GizmoUtilities.DrawArrow(left, ArrowDirection.Left);
                            break;
                        case ConnectionType.Right:
                            GizmoUtilities.DrawArrow(right, ArrowDirection.Right);
                            break;
                    }
                }
            }

            if (!(!_showEdgesWhenPlay && Application.isPlaying) && Enviorment)
            {
                Gizmos.color = Color.white;
                Vector2 gridSize = new Vector2(Width, Height);
                Vector2 cellSize = Enviorment.cellSize;

                float yMin = transform.position.y - gridSize.y * cellSize.y / 2;
                float yMax = transform.position.y + gridSize.y * cellSize.y / 2;
                float xMin = transform.position.x - gridSize.x * cellSize.x / 2;
                float xMax = transform.position.x + gridSize.x * cellSize.x / 2;

                Gizmos.DrawLine(new Vector3(xMin, yMin), new Vector3(xMin, yMax));
                Gizmos.DrawLine(new Vector3(xMax, yMin), new Vector3(xMax, yMax));
                Gizmos.DrawLine(new Vector3(xMin, yMin), new Vector3(xMax, yMin));
                Gizmos.DrawLine(new Vector3(xMin, yMax), new Vector3(xMax, yMax));
            }
        }

    }
}
