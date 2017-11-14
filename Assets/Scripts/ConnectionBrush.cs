using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MapGeneration
{
    public enum ConnectionType
    {
        Top, Bottom, Left, Right
    }

    [CustomGridBrush(false, true, false, "ConnectionBrush")]
    public class ConnectionBrush : GridBrush
    {
        
        public ConnectionType BrushConnectionType { get; set; }
#if UNITY_EDITOR
        [MenuItem("Assets/Create/Brushes/ConnectionBrush")]
        //This Function is called when you click the menu entry
        private static void CreateConnectionBrush()
        {
            string path = EditorUtility.SaveFilePanelInProject("Sace ConnectionBrush",
                "ConnectionBrush", "asset", "Save ConnectionBrush", "Assets");

            if(path != "")
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ConnectionBrush>(),path);

        }
#endif
        

        public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            Chunk chunk = brushTarget.GetComponent<Chunk>() ??
                          brushTarget.GetComponentInParent<Chunk>();
            
            if (chunk)
            {
                Connection connection = chunk.Connections.FirstOrDefault(x => x.Position == position);
                if (connection != null)
                    connection.Type = BrushConnectionType;
                else
                    chunk.Connections.Add(new Connection(position,BrushConnectionType));
            }
            base.Paint(gridLayout, brushTarget, position);
        }


        public override void Erase(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            Chunk chunk = brushTarget.GetComponent<Chunk>() ??
                          brushTarget.GetComponentInParent<Chunk>();

            if (chunk)
            {
                Connection connection = chunk.Connections.FirstOrDefault(x => x.Position == position);
                if (connection != null)
                    chunk.Connections.Remove(connection);
            }
            base.Erase(gridLayout, brushTarget, position);


        }
    }

}


