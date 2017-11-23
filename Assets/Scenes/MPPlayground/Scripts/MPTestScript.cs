using MapGeneration;
using UnityEngine;

namespace MPPlayground
{
    public class MPTestScript : MonoBehaviour
    {
        void Start()
        {
            if (!MapBuilder.Instance.PreExistingMap)
                MapCycler.Instance.LoadNextMap();
        }
    }
}