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



        [SerializeField] private bool _drawBacktracking = true;

        

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


