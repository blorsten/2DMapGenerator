using System;
using System.Collections;
using System.Collections.Generic;
using MapGeneration;
using MapGeneration.Extensions;
using UnityEngine;

namespace MapGeneration.Utils
{
    [ExecuteInEditMode]
    public class MapGizmos : MonoBehaviour
    {
        private const string TRAP_ICON_PATH = "Trap.png";
        private const string TREASURE_ICON_PATH = "Treasure.png";
        private const string GROUND_ICON_PATH = "Ground.png";
        private const string FLYING_ICON_PATH = "Flying.png";


        [Header("Draw Booleans"), SerializeField] private bool _drawConnections = true;
        [SerializeField] private bool _drawBacktracking = true;
        [SerializeField] private bool _drawEdges = true;
        [SerializeField] private bool _drawTileFlags = true;
        

        private Map _map;

        public Map Map
        {
            get
            {
                if (!_map)
                    _map = GetComponent<Map>();
                return _map;
            }
            set { _map = value; }
        }

        public void OnDrawGizmos()
        {
            if (Map == null || Map.Grid == null)
                return;
            for (int x = 0; x < Map.Grid.GetLength(0); x++)
            {
                for (int y = 0; y < Map.Grid.GetLength(1); y++)
                {
                    if(Map.Grid[x, y].Instance == null)
                        continue;
                    Chunk chunk = Map.Grid[x, y].Instance;
                    DrawBackTracking(chunk);
                    DrawConnections(chunk);
                    DrawEdges(chunk);
                    DrawFlags(chunk);
                }
            }
            
        }

        private void DrawFlags(Chunk chunk)
        {
            if (!_drawTileFlags)
                return;
            foreach (var flag in chunk.TileFlags)
            {
                Vector3 postition = chunk.Enviorment.GetCellCenterWorld(flag.Position);

                switch (flag.Type)
                {
                    case TileType.Trap:
                        UnityEngine.Gizmos.DrawIcon(postition, TRAP_ICON_PATH, true);
                        break;
                    case TileType.Treasure:
                        UnityEngine.Gizmos.DrawIcon(postition,TREASURE_ICON_PATH, true);
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

        private void DrawEdges(Chunk chunk)
        {
            if (_drawEdges && chunk.Enviorment)
            {
                UnityEngine.Gizmos.color = Color.white;
                Vector2 gridSize = new Vector2(chunk.Width, chunk.Height);
                Vector2 cellSize = chunk.Enviorment.cellSize;

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

        private void DrawConnections(Chunk chunk)
        {
            if (_drawConnections && chunk.Enviorment)
            {
                UnityEngine.Gizmos.color = new Color(231f / 255f, 76f / 255f, 60f / 255f);

                foreach (var c in chunk.Connections)
                {
                    Vector3 cellPosition = chunk.Enviorment.GetCellCenterWorld(c.Position);
                    Vector2 cellSize = chunk.Enviorment.cellSize;

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

        private void DrawBackTracking(Chunk chunk)
        {
            if (chunk.ChunkHolder != null && _drawBacktracking && chunk.Enviorment)
            {
                UnityEngine.Gizmos.color = Color.red;

                if (chunk.ChunkHolder.ChunkOpenings.TopConnection)
                    UnityEngine.Gizmos.DrawLine(chunk.transform.position,
                        chunk.transform.position + Vector3.up * (chunk.Height / 2f));
                if (chunk.ChunkHolder.ChunkOpenings.BottomConnetion)
                    UnityEngine.Gizmos.DrawLine(chunk.transform.position,
                        chunk.transform.position + Vector3.down * (chunk.Height / 2f));
                if (chunk.ChunkHolder.ChunkOpenings.RightConnection)
                    UnityEngine.Gizmos.DrawLine(chunk.transform.position,
                        chunk.transform.position + Vector3.right * (chunk.Width / 2f));
                if (chunk.ChunkHolder.ChunkOpenings.LeftConnection)
                    UnityEngine.Gizmos.DrawLine(chunk.transform.position,
                        chunk.transform.position + Vector3.left * (chunk.Width / 2f));
            }
        }
    }

}


