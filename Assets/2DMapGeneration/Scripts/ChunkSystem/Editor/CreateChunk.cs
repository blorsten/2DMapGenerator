using MapGeneration.ChunkSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapGeneration
{
    /// <summary>
    /// This class is used to instantiate a new chunk.
    /// </summary>
    public class CreateChunk : MonoBehaviour
    {
        /// <summary>
        /// This method instantiates a new chunk.
        /// </summary>
        [MenuItem("GameObject/2D Map Generation/Chunk", false, 0)]
        public static void CreateDefaultChunk()
        {
            var prefab = AssetDatabase.LoadAssetAtPath("Assets/2DMapGeneration/Templates/New Chunk.prefab", typeof(GameObject)) as GameObject;
            var newGo = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newGo.GetComponent<Chunk>().Environment = newGo.GetComponentInChildren<Tilemap>(); 

        }
    }
}