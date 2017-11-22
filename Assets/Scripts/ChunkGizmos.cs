using System;
using System.Collections;
using System.Collections.Generic;
using MapGeneration;
using MapGeneration.Extensions;
using UnityEngine;

namespace MapGeneration.Gizmos
{
    [ExecuteInEditMode]
    public class ChunkGizmos : MonoBehaviour
    {

        [Header("References"),SerializeField]private GizmoIcons _gizmos;

        [Header("Draw Booleans"), SerializeField] private bool _drawConnections = true;
        [SerializeField] private bool _drawBacktracking = true;
        [SerializeField] private bool _drawEdges = true;
        [SerializeField] private bool _drawTileFlags = true;
        

        private Chunk _chunk;

        public Chunk Chunk
        {
            get
            {
                if (!_chunk)
                    _chunk = GetComponent<Chunk>();
                return _chunk;
            }
        }

        public void Awake()
        {
            if (!_gizmos)
                Debug.LogWarning(gameObject.name + " is missing a " + typeof(GizmoIcons).Name);
        }

        public void OnDrawGizmos()
        {
            if (!Chunk)
                return;

            DrawBackTracking();
            DrawConnections();
            DrawEdges();
            DrawFlags();
        }

        private void DrawFlags()
        {
            if (!_gizmos || !_drawTileFlags)
                return;
            foreach (var flag in Chunk.TileFlags)
            {
                Vector3 postition = Chunk.Enviorment.GetCellCenterWorld(flag.Position);

                switch (flag.Type)
                {
                    case TileType.Trap:
                        UnityEngine.Gizmos.DrawIcon(postition, _gizmos.TrapIcon, true);
                        break;
                    case TileType.Treasure:
                        UnityEngine.Gizmos.DrawIcon(postition, _gizmos.TreasureIcon, true);
                        break;
                    case TileType.FlyingSpawn:
                        UnityEngine.Gizmos.DrawIcon(postition, _gizmos.FlyingIcon, true);
                        break;
                    case TileType.GroundSpawn:
                        UnityEngine.Gizmos.DrawIcon(postition, _gizmos.GroundIcon, true);
                        break;

                }
            }
        }

        private void DrawEdges()
        {
            if (_drawEdges && Chunk.Enviorment)
            {
                UnityEngine.Gizmos.color = Color.white;
                Vector2 gridSize = new Vector2(Chunk.Width, Chunk.Height);
                Vector2 cellSize = Chunk.Enviorment.cellSize;

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

        private void DrawConnections()
        {
            if (_drawConnections && Chunk.Enviorment)
            {
                UnityEngine.Gizmos.color = new Color(231f / 255f, 76f / 255f, 60f / 255f);

                foreach (var c in Chunk.Connections)
                {
                    Vector3 cellPosition = Chunk.Enviorment.GetCellCenterWorld(c.Position);
                    Vector2 cellSize = Chunk.Enviorment.cellSize;

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
        }

        private void DrawBackTracking()
        {
            if (Chunk.ChunkHolder != null && _drawBacktracking && Chunk.Enviorment)
            {
                UnityEngine.Gizmos.color = Color.red;

                if (Chunk.ChunkHolder.ChunkOpenings.TopConnection)
                    UnityEngine.Gizmos.DrawLine(transform.position,
                        transform.position + Vector3.up * (Chunk.Height / 2f));
                if (Chunk.ChunkHolder.ChunkOpenings.BottomConnetion)
                    UnityEngine.Gizmos.DrawLine(transform.position,
                        transform.position + Vector3.down * (Chunk.Height / 2f));
                if (Chunk.ChunkHolder.ChunkOpenings.RightConnection)
                    UnityEngine.Gizmos.DrawLine(transform.position,
                        transform.position + Vector3.right * (Chunk.Width / 2f));
                if (Chunk.ChunkHolder.ChunkOpenings.LeftConnection)
                    UnityEngine.Gizmos.DrawLine(transform.position,
                        transform.position + Vector3.left * (Chunk.Width / 2f));
            }
        }
    }

}


