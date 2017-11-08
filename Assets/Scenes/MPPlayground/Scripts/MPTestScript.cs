using MapGeneration;
using UnityEngine;

namespace MPPlayground
{
    public class MPTestScript : MonoBehaviour 
    {
        void Start()
        {
            MapBuilder.Instance.Generate(new MapBlueprint());
        }
    }
}