using MapGeneration.Algorithm;
using MapGeneration.ChunkSystem;
using UnityEngine;

namespace MapGeneration.Utils
{
    [ExecuteInEditMode]
    public class MapGizmos : MonoBehaviour
    {
        [SerializeField] private bool _drawBacktracking = true;

        [SerializeField] private Map _map; 

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
                }
            }

            DrawMapEdge();
        }

        private void DrawMapEdge()
        {
            Gizmos.color = Color.red;
            float xMin = transform.position.x;
            float xMax = transform.position.x + _map.MapBlueprint.GridSize.x * _map.MapBlueprint.ChunkSize.x;
            float yMin = transform.position.y;
            float yMax = transform.position.y + _map.MapBlueprint.GridSize.y * _map.MapBlueprint.ChunkSize.y;

            Gizmos.DrawLine(new Vector3(xMin,yMin), new Vector3(xMax,yMin));
            Gizmos.DrawLine(new Vector3(xMax, yMin), new Vector3(xMax, yMax));
            Gizmos.DrawLine(new Vector3(xMax, yMax), new Vector3(xMin, yMax));
            Gizmos.DrawLine(new Vector3(xMin, yMax), new Vector3(xMin, yMin));
        }

        private void DrawBackTracking(Chunk chunk)
        {
            if (chunk.ChunkHolder != null && _drawBacktracking && chunk.Enviorment)
            {
                if (chunk.ChunkHolder.ChunkOpenings.TopConnection)
                    DrawBacktrackingLine(chunk, PathAlgorithm.CardinalDirections.Top, chunk.ChunkHolder.ChunkOpenings.TopConnectionType);
                if (chunk.ChunkHolder.ChunkOpenings.BottomConnetion)
                    DrawBacktrackingLine(chunk, PathAlgorithm.CardinalDirections.Bottom, chunk.ChunkHolder.ChunkOpenings.BottomConnectionType);
                if (chunk.ChunkHolder.ChunkOpenings.RightConnection)
                    DrawBacktrackingLine(chunk, PathAlgorithm.CardinalDirections.Right, chunk.ChunkHolder.ChunkOpenings.RightConnectionType);
                if (chunk.ChunkHolder.ChunkOpenings.LeftConnection)
                    DrawBacktrackingLine(chunk, PathAlgorithm.CardinalDirections.Left, chunk.ChunkHolder.ChunkOpenings.LeftConnectionType);
            }
        }

        private void DrawBacktrackingLine(Chunk chunk, PathAlgorithm.CardinalDirections dir, ChunkOpenings.ConnectionType type)
        {
            Color chosenColor = Color.white;

            switch (type)
            {
                case ChunkOpenings.ConnectionType.Default:
                    chosenColor = MapBuilder.Settings.DefaultConnectionColor;
                    break;
                case ChunkOpenings.ConnectionType.Critical:
                    chosenColor = MapBuilder.Settings.CriticalConnectionColor;
                    break;
            }

            Vector3 middleAdder = new Vector2(chunk.Width/2,chunk.Height/2);

            switch (dir)
            {
                case PathAlgorithm.CardinalDirections.Top:
                    Gizmos.color = chosenColor;
                    Gizmos.DrawLine(chunk.transform.position + middleAdder,
                        chunk.transform.position + Vector3.up * (chunk.Height / 2f) + middleAdder);
                    break;
                case PathAlgorithm.CardinalDirections.Bottom:
                    Gizmos.color = chosenColor;
                    Gizmos.DrawLine(chunk.transform.position + middleAdder,
                        chunk.transform.position + Vector3.down * (chunk.Height / 2f) + middleAdder);
                    break;
                case PathAlgorithm.CardinalDirections.Left:
                    Gizmos.color = chosenColor;
                    Gizmos.DrawLine(chunk.transform.position + middleAdder,
                        chunk.transform.position + Vector3.left * (chunk.Width / 2f) + middleAdder);
                    break;
                case PathAlgorithm.CardinalDirections.Right:
                    Gizmos.color = chosenColor;
                    Gizmos.DrawLine(chunk.transform.position + middleAdder,
                        chunk.transform.position + Vector3.right * (chunk.Width / 2f) + middleAdder);
                    break;
            }
        }
    }

}


