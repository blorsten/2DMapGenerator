using MapGeneration;
using UnityEngine;

namespace MPPlayground
{
    public class MPTestScript : MonoBehaviour
    {

        void Start()
        {
            MapBuilder.Instance.Generate();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
                MapBuilder.Instance.Generate();
        }
    }
}