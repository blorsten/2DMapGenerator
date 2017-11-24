using UnityEditor;
using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// Purpose:
    /// Creator:
    /// </summary>
    public class CreateChunk : MonoBehaviour
    {
        [MenuItem("Assets/Create/MapGeneration/Chunk")]
        public static void CreateDefaultChunk()
        {
            var prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Templates/defaultChunk.prefab", typeof(GameObject)) as GameObject;

            Instantiate(prefab, Vector3.zero, Quaternion.identity);

        }
    }
}