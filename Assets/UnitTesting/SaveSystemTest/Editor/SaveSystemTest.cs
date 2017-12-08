using System.Collections;
using MapGeneration;
using MapGeneration.SaveSystem;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.TestTools;

[TestFixture(Author = "MP", Category = "SaveSystem")]
public class SaveSystemTest
{
    public SaveSystemTest()
    {
        MapBuilder.Instance.CurrentBlueprint =
            AssetDatabase.LoadAssetAtPath<MapBlueprint>("Assets/UnitTesting/SaveSystemTest/SaveSystemBPTest.asset");
    }

    [Test]
    public void MapDataSaver_Construction_And_Initalization()
    {
        Map newMap = MapBuilder.Instance.Generate();
        
        Assert.AreEqual(newMap, newMap.MapDataSaver.Map);
        Assert.AreEqual(newMap.ID, newMap.MapDataSaver.MapId);
        Assert.AreEqual(newMap.Seed, newMap.MapDataSaver.MapSeed);
        Assert.AreEqual(newMap.MapBlueprint, newMap.MapDataSaver.MapBlueprint);
    }

    [Test]
	public void MapDataSaver_Save()
    {
        Map newMap = MapBuilder.Instance.Generate();

        newMap.GetComponentInChildren<SaveSystemTestScript>().TestData = 1337;

        newMap.MapDataSaver.SavePersistentData();

        Assert.IsTrue(newMap.MapDataSaver.HasSavedData);
    }

    [Test]
    public void MapDataSaver_Load_With_Manual()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestData = 1337;

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual(1337, newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestData);
    }

    [Test]
    public void MapDataSaver_Load_With_Generation()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestData = 1337;

        Map secondMap = MapBuilder.Instance.Generate(newMap);

        Assert.AreEqual(1337, secondMap.GetComponentInChildren<SaveSystemTestScript>().TestData);
    }
}
