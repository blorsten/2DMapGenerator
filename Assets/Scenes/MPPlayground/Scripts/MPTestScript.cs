using MapGeneration;
using UnityEngine;

namespace MPPlayground
{
    public class MPTestScript : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MapCycler.Instance.LoadNextMap();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MapCycler.Instance.LoadPreviousMap();
            }
        }
    }
}