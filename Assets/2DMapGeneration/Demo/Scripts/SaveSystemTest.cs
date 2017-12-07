using MapGeneration.SaveSystem;
using UnityEngine;

namespace MapGeneration
{
    public class SaveSystemTest : MonoBehaviour
    {
        [SerializeField, PersistentData]
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private int _testField = 1337;

        void Start()
        {
            Debug.Log(_testField);
        }
    } 
}