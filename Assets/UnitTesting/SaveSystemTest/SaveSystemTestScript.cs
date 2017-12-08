using MapGeneration.SaveSystem;
using UnityEngine;

public class SaveSystemTestScript : MonoBehaviour 
{
    [SerializeField, PersistentData] private int _testData;

    public int TestData
    {
        get { return _testData; }
        set { _testData = value; }
    }
}
