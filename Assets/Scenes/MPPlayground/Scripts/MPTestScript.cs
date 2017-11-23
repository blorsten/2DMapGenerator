using MapGeneration;
using UnityEngine;

namespace MPPlayground
{
    public class MPTestScript : MonoBehaviour
    {
        void Start()
        {
            MapCycler.Instance.LoadNextMap();
        }
    }
}