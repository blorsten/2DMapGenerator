using MapGeneration;
using UnityEngine;

namespace MPPlayground
{
    public class MPTestScript : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
                MapBuilder.Instance.Generate();
        }
    }
}