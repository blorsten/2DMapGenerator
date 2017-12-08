using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using MapGeneration;
using MapGeneration.Algorithm;

[TestFixture(Author = "Niels", Category = "PathAlgorithms")]
public class AlgorithmTest
{
    private const string MAPBUILER_TEST_BLUEPRINT_PATH =
        "Assets/UnitTesting/AglorithmsTest/AlgorithmBPTest.asset";

    MapBlueprint _bp;
    SpelunkyAlgorithm _spelunky;
    DrunkardWalkAlgorithm _drunkard;
    DeadEndMaker _deadEndMaker;
    PerlinNoiseBiome _perlinNoise;
    VoronoiBiome _voronoi;

    public AlgorithmTest()
    {
        MapBuilder.Instance.CurrentBlueprint = AssetDatabase.LoadAssetAtPath<MapBlueprint>(MAPBUILER_TEST_BLUEPRINT_PATH);
        _bp = MapBuilder.Instance.CurrentBlueprint;
        _spelunky = SpelunkyAlgorithm.CreateInstance<SpelunkyAlgorithm>();
        _drunkard = DrunkardWalkAlgorithm.CreateInstance<DrunkardWalkAlgorithm>();
        _deadEndMaker = DeadEndMaker.CreateInstance<DeadEndMaker>();
        _perlinNoise = PerlinNoiseBiome.CreateInstance<PerlinNoiseBiome>();
        _voronoi = VoronoiBiome.CreateInstance<VoronoiBiome>();
    }

    [Test]
    public void Algorithm_In_Stack()
    {
        AlgorithmStorage storage = new AlgorithmStorage(_spelunky);
        _bp.AlgorithmStack.Add(storage);

        Assert.AreEqual(storage.Algorithm, _spelunky);
    }

    [Test]
    public void ChunkPlacer_Execution()
    {
        AlgorithmStorage spelunkyStorage = new AlgorithmStorage(_spelunky);

        _bp.AlgorithmStack.Add(spelunkyStorage);

        bool result = MapBuilder.Instance.Generate(_bp);

        Assert.IsTrue(result);
    }

    [Test]
    public void Spelunky_Algorithm_Execution()
    {
        AlgorithmStorage storage = new AlgorithmStorage(_spelunky);
        _bp.AlgorithmStack.Add(storage);

        MapBuilder.Instance.Generate(_bp);

        Assert.IsTrue(MapBuilder.Instance.ActiveMap);
    }

    [Test]
    public void DrunkardWalk_Algorithm_Execution()
    {
        AlgorithmStorage storage = new AlgorithmStorage(_drunkard);
        _bp.AlgorithmStack.Add(storage);

        MapBuilder.Instance.Generate(_bp);

        Assert.IsTrue(MapBuilder.Instance.ActiveMap);
    }

    [Test]
    public void DeadEndMaker_Algorithm()
    {
        AlgorithmStorage spelunkyStorage = new AlgorithmStorage(_spelunky);
        AlgorithmStorage deadEndStorage = new AlgorithmStorage(_deadEndMaker);
        _bp.AlgorithmStack.Add(spelunkyStorage);
        _bp.AlgorithmStack.Add(deadEndStorage);

        MapBuilder.Instance.Generate(_bp);

        Assert.IsTrue(MapBuilder.Instance.ActiveMap);
    }

    [Test]
    public void Biome_In_Stack()
    {
        AlgorithmStorage spelunkyStorage = new AlgorithmStorage(_spelunky);
        AlgorithmStorage deadEndStorage = new AlgorithmStorage(_deadEndMaker);
        AlgorithmStorage biomeStorage = new AlgorithmStorage(_perlinNoise);

        _bp.AlgorithmStack.Add(spelunkyStorage);
        _bp.AlgorithmStack.Add(deadEndStorage);
        _bp.AlgorithmStack.Add(biomeStorage);

        MapBuilder.Instance.Generate(_bp);

        Assert.AreEqual(biomeStorage.Algorithm, _perlinNoise);
    }

    [Test]
    public void Perlin_Noise_Biome_Execution()
    {
        AlgorithmStorage spelunkyStorage = new AlgorithmStorage(_spelunky);
        AlgorithmStorage deadEndStorage = new AlgorithmStorage(_deadEndMaker);
        AlgorithmStorage biomeStorage = new AlgorithmStorage(_perlinNoise);

        _bp.AlgorithmStack.Add(spelunkyStorage);
        _bp.AlgorithmStack.Add(deadEndStorage);
        _bp.AlgorithmStack.Add(biomeStorage);

        MapBuilder.Instance.Generate(_bp);

        Assert.IsTrue(MapBuilder.Instance.ActiveMap);
    }

    [Test]
    public void Voronoi_Biome_Execution()
    {
        AlgorithmStorage spelunkyStorage = new AlgorithmStorage(_spelunky);
        AlgorithmStorage deadEndStorage = new AlgorithmStorage(_deadEndMaker);
        AlgorithmStorage biomeStorage = new AlgorithmStorage(_voronoi);

        _bp.AlgorithmStack.Add(spelunkyStorage);
        _bp.AlgorithmStack.Add(deadEndStorage);
        _bp.AlgorithmStack.Add(biomeStorage);

        MapBuilder.Instance.Generate(_bp);

        Assert.IsTrue(MapBuilder.Instance.ActiveMap);
    }
}
