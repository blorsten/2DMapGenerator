using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// This brush is used to place tiles in a chunks tiledata list
    /// </summary>
    [CustomGridBrush(false, true, false, "TileTypeBrush")]
    public class TileTypeBrush : GridBrush
    {
        //The current tiletype, when be used when a tile is placed
        public TileType BrushTileType { get; set; }
        
        public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            //This tries to get a chunk component from the brush target
            Chunk chunk = brushTarget.GetComponent<Chunk>() ??
                          brushTarget.GetComponentInParent<Chunk>();
            
            //If a chunk was found, place a tile in the tiledata list
            if (chunk)
            {
                //If a chunk in the tiledata list allready this position, replace it else create new
                Tile tile = chunk.TileData.FirstOrDefault(x => x.Position == position);
                if (tile != null)
                {
                    tile.Type = BrushTileType;
                    tile.Chunk = chunk;
                }
                else
                    chunk.TileData.Add(new Tile(position,BrushTileType,chunk));
            }
        }


        public override void Erase(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            //This tries to get a chunk component from the brush target
            Chunk chunk = brushTarget.GetComponent<Chunk>() ??
                          brushTarget.GetComponentInParent<Chunk>();
           
            //If a chunk is found, erase a tile in the postion
            if (chunk)
            {
                Tile tile = chunk.TileData.FirstOrDefault(x => x.Position == position);
                if (tile != null)
                    chunk.TileData.Remove(tile);
            }


        }
#if UNITY_EDITOR
        [MenuItem("Assets/Create/Brushes/TileTypeBrush")]
        //This created the brush as a scriptableObject
        private static void CreateConnectionBrush()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save TileTypeBrush",
                "TileTypeBrush", "asset", "Save TileTypeBrush", "Assets");

            if (path != "")
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<TileTypeBrush>(), path);

        }
#endif
    }

}


