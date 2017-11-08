using MapGeneration;
using UnityEngine;

namespace MPPlayground
{
    public class MPTestScript : MonoBehaviour 
    {
        void Start()
        {
            Debug.Log(ResourceHandler.Instance.Chunks.Count);
        }
    }
}