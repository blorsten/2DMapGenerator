using System.Collections;
using System.Collections.Generic;
using MapGeneration;
using MapGeneration.Extensions;
using UnityEngine;

[ExecuteInEditMode]
public class ChunkGizmos : MonoBehaviour
{

    [Header("Gizmos"), SerializeField] private bool _showConnectionWhenPlay = true;
    [SerializeField] private bool _showBacktrackingWhenPlay = true;
    [SerializeField] private bool _showEdgesWhenPlay = true;

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


    private void OnDrawGizmos()
    {
        if(!Chunk)
            return;

        DrawBackTracking();

        DrawConnections();

        DrawEdges();
    }

    private void DrawEdges()
    {
        if (!(!_showEdgesWhenPlay && Application.isPlaying) && Chunk.Enviorment)
        {
            Gizmos.color = Color.white;
            Vector2 gridSize = new Vector2(Chunk.Width, Chunk.Height);
            Vector2 cellSize = Chunk.Enviorment.cellSize;

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

    private void DrawConnections()
    {
        if (!(!_showConnectionWhenPlay && Application.isPlaying) && Chunk.Enviorment)
        {
            Gizmos.color = new Color(231f / 255f, 76f / 255f, 60f / 255f);

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
        if (Chunk.ChunkHolder != null && !(!_showBacktrackingWhenPlay && Application.isPlaying))
        {
            Gizmos.color = Color.red;

            if (Chunk.ChunkHolder.ChunkOpenings.TopConnection)
                Gizmos.DrawLine(transform.position,
                    transform.position + Vector3.up * (Chunk.Height / 2f));
            if (Chunk.ChunkHolder.ChunkOpenings.BottomConnetion)
                Gizmos.DrawLine(transform.position,
                    transform.position + Vector3.down * (Chunk.Height / 2f));
            if (Chunk.ChunkHolder.ChunkOpenings.RightConnection)
                Gizmos.DrawLine(transform.position,
                    transform.position + Vector3.right * (Chunk.Width / 2f));
            if (Chunk.ChunkHolder.ChunkOpenings.LeftConnection)
                Gizmos.DrawLine(transform.position,
                    transform.position + Vector3.left * (Chunk.Width / 2f));
        }
    }
}
