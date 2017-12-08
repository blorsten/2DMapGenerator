using MapGeneration;
using NUnit.Framework;
using UnityEditor;

[TestFixture(Author = "MP", Category = "SaveSystem")]
public class SaveSystemTest
{
    public SaveSystemTest()
    {
        MapBuilder.Instance.CurrentBlueprint =
            AssetDatabase.LoadAssetAtPath<MapBlueprint>("Assets/UnitTesting/SaveSystemTest/SaveSystemBPTest.asset");
    }

	[Test]
	public void Save()
    {

	}

    public void Load()
    {
        
    }
}
