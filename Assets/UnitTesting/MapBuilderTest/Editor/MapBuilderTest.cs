using System;
using System.Collections;
using MapGeneration;
using MapGeneration.ChunkSystem;
using MapGeneration.SaveSystem;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.TestTools;

[TestFixture(Author = "MP", Category = "MapBuilder")]
public class MapBuilderTest
{
    private MapBlueprint _testBlueprint;

    public MapBuilderTest()
    {
        MapBuilder.Instance.CurrentBlueprint =
            AssetDatabase.LoadAssetAtPath<MapBlueprint>("Assets/UnitTesting/MapBuilderTest/MapBuilderBPTest.asset");
    }

    [Test]
    public void Blueprint_Acceptance()
    {
        Assert.IsTrue(MapBuilder.Instance.CurrentBlueprint);
    }

    [Test]
    public void Generate()
    {
        MapBuilder.Instance.Generate();

        Assert.IsTrue(MapBuilder.Instance.ActiveMap);
    }

    [Test]
    public void Generate_With_Seed()
    {
        MapBuilder.Instance.Generate(100);

        Assert.AreEqual(100, MapBuilder.Instance.ActiveMap.Seed);
    }

    [Test]
    public void Generate_Existing_Map()
    {
        MapDataSaver existingMap = MapBuilder.Instance.Generate().MapDataSaver;

        Guid savedID = existingMap.MapId;

        MapBuilder.Instance.Generate(existingMap);

        Assert.IsTrue(MapBuilder.Instance.SavedMaps.Contains(existingMap));
        Assert.IsTrue(MapBuilder.Instance.ActiveMap.ID == savedID);
    }

    [Test]
    public void Spawn_Map()
    {
        Map newMap = MapBuilder.Instance.Generate();

        MapBuilder.Instance.Spawn(newMap);

        foreach (ChunkHolder chunkHolder in newMap.Grid)
            Assert.IsTrue(chunkHolder.Instance);
    }

    [Test]
    public void Despawn_Map()
    {
        Map newMap = MapBuilder.Instance.Generate();

        MapBuilder.Instance.Despawn(newMap);

        Assert.IsFalse(newMap);
    }

    [Test]
    public void Save_Map()
    {
        Map newMap = MapBuilder.Instance.Generate();

        Assert.IsTrue(MapBuilder.Instance.SavedMaps.Contains(newMap.MapDataSaver));
    }

    [Test]
    public void Settings_Loaded()
    {
        Assert.IsTrue(MapBuilder.Settings);
    }

    [UnityTest]
    public IEnumerator MapSpawned_Event()
    {
        bool evaluationComplete = false;

        MapBuilder.Instance.MapSpawned += map =>
        {
            evaluationComplete = true;
        };

        MapBuilder.Instance.Generate();

        yield return null;

        Assert.IsTrue(evaluationComplete);
    }
}