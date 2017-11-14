using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MapGeneration
{
    public enum ConnectionType
    {
        Top, Bottom, Left, Right
    }

    [CustomGridBrush(false, true, false, "ConnectionBrush")]
    public class ConnectionBrush : GridBrushBase
    {
        
        public ConnectionType BrushConnectionType { get; set; }



#if UNITY_EDITOR
        [MenuItem("Assets/Create/Brushes/ConnectionBrush")]
        //This Function is called when you click the menu entry
        private static void CreateConnectionBrush()
        {
            string path = EditorUtility.SaveFilePanelInProject("Sace ConnectionTile",
                "ConnectionTile", "asset", "Save ConnectionTIle", "Assets");

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
                switch (BrushConnectionType)
                {
                    case ConnectionType.Top:
                        if(!chunk.TopConnections.Contains(position))
                            chunk.TopConnections.Add(position);
                        break;
                    case ConnectionType.Bottom:
                        if (!chunk.BottomConnections.Contains(position))
                            chunk.BottomConnections.Add(position);
                        break;
                    case ConnectionType.Left:
                        if (!chunk.LeftConnections.Contains(position))
                            chunk.LeftConnections.Add(position);
                        break;
                    case ConnectionType.Right:
                        if (!chunk.RightConnections.Contains(position))
                            chunk.RightConnections.Add(position);
                        break;
                }
            }
            base.Paint(gridLayout, brushTarget, position);

        }


        public override void Erase(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            Chunk chunk = brushTarget.GetComponent<Chunk>() ??
                          brushTarget.GetComponentInParent<Chunk>();
            if (chunk)
            {
                switch (BrushConnectionType)
                {
                    case ConnectionType.Top:
                        if (chunk.TopConnections.Contains(position))
                            chunk.TopConnections.Remove(position);
                        break;
                    case ConnectionType.Bottom:
                        if (chunk.BottomConnections.Contains(position))
                            chunk.BottomConnections.Remove(position);
                        break;
                    case ConnectionType.Left:
                        if (chunk.LeftConnections.Contains(position))
                            chunk.LeftConnections.Remove(position);
                        break;
                    case ConnectionType.Right:
                        if (chunk.RightConnections.Contains(position))
                            chunk.RightConnections.Remove(position);
                        break;
                }
            }
            base.Erase(gridLayout, brushTarget, position);


        }
    }

}


