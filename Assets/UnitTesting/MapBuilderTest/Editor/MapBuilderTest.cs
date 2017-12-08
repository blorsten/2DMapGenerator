using System;
using System.Collections;
using MapGeneration;
using MapGeneration.ChunkSystem;
using MapGeneration.SaveSystem;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class MapBuilderTest
{
    private const string MAPBUILER_TEST_BLUEPRINT_PATH =
        "Assets/UnitTesting/MapBuilderTest/MapBuilderBPTest.asset";

    [Test]
	public void MapBuilder_Blueprint_Acceptance()
    {
        MapBuilder.Instance.CurrentBlueprint =
            AssetDatabase.LoadAssetAtPath<MapBlueprint>(MAPBUILER_TEST_BLUEPRINT_PATH);

        Assert.IsTrue(MapBuilder.Instance.CurrentBlueprint);
	}

    [Test]
    public void MapBuilder_Generate()
    {
        MapBuilder.Instance.CurrentBlueprint =
            AssetDatabase.LoadAssetAtPath<MapBlueprint>(MAPBUILER_TEST_BLUEPRINT_PATH);

        MapBuilder.Instance.Generate();

        Assert.IsTrue(MapBuilder.Instance.ActiveMap);
    }

    [Test]
    public void MapBuilder_Generate_With_Seed()
    {
        MapBuilder.Instance.CurrentBlueprint =
            AssetDatabase.LoadAssetAtPath<MapBlueprint>(MAPBUILER_TEST_BLUEPRINT_PATH);

        MapBuilder.Instance.Generate(100);

        Assert.AreEqual(100, MapBuilder.Instance.ActiveMap.Seed);
    }

    [Test]
    public void MapBuilder_Generate_Existing_Map()
    {
        MapBuilder.Instance.CurrentBlueprint =
            AssetDatabase.LoadAssetAtPath<MapBlueprint>(MAPBUILER_TEST_BLUEPRINT_PATH);

        MapDataSaver existingMap = MapBuilder.Instance.Generate().MapDataSaver;

        Guid savedID = existingMap.MapId;

        MapBuilder.Instance.Generate(existingMap);

        Assert.IsTrue(MapBuilder.Instance.SavedMaps.Contains(existingMap));
        Assert.IsTrue(MapBuilder.Instance.ActiveMap.ID == savedID);
    }

    [Test]
    public void MapBuilder_Spawn_Map()
    {
        MapBuilder.Instance.CurrentBlueprint =
            AssetDatabase.LoadAssetAtPath<MapBlueprint>(MAPBUILER_TEST_BLUEPRINT_PATH);

        Map newMap = MapBuilder.Instance.Generate();
        
        MapBuilder.Instance.Spawn(newMap);

        foreach (ChunkHolder chunkHolder in newMap.Grid)
            Assert.IsTrue(chunkHolder.Instance);
    }

    [Test]
    public void MapBuilder_Despawn_Map()
    {
        MapBuilder.Instance.CurrentBlueprint =
            AssetDatabase.LoadAssetAtPath<MapBlueprint>(MAPBUILER_TEST_BLUEPRINT_PATH);

        Map newMap = MapBuilder.Instance.Generate();

        MapBuilder.Instance.Despawn(newMap);
        
        Assert.IsFalse(newMap.gameObject);
    }

    [Test]
    public void MapBuilder_Save_Map()
    {
        MapBuilder.Instance.CurrentBlueprint =
            AssetDatabase.LoadAssetAtPath<MapBlueprint>(MAPBUILER_TEST_BLUEPRINT_PATH);

        Map newMap = MapBuilder.Instance.Generate();

        Assert.IsTrue(MapBuilder.Instance.SavedMaps.Contains(newMap.MapDataSaver));
    }

    [Test]
    public void MapBuilder_Settings_Loaded()
    {
        Assert.IsTrue(MapBuilder.Settings);
    }

    [UnityTest]
    public IEnumerator MapBuilder_MapSpawned_Event()
    {
        MapBuilder.Instance.CurrentBlueprint =
            AssetDatabase.LoadAssetAtPath<MapBlueprint>(MAPBUILER_TEST_BLUEPRINT_PATH);

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
