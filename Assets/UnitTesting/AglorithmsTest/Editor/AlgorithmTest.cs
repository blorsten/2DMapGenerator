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

    private readonly MapBlueprint _recipeBp;
    private MapBlueprint _bp;
    private readonly SpelunkyAlgorithm _spelunky;
    private readonly DrunkardWalkAlgorithm _drunkard;
    private readonly DeadEndMaker _deadEndMaker;
    private readonly PerlinNoiseBiome _perlinNoise;
    private readonly VoronoiBiome _voronoi;

    public AlgorithmTest()
    {
        _recipeBp = AssetDatabase.LoadAssetAtPath<MapBlueprint>(MAPBUILER_TEST_BLUEPRINT_PATH);
        _spelunky = SpelunkyAlgorithm.CreateInstance<SpelunkyAlgorithm>();
        _drunkard = DrunkardWalkAlgorithm.CreateInstance<DrunkardWalkAlgorithm>();
        _deadEndMaker = DeadEndMaker.CreateInstance<DeadEndMaker>();
        _perlinNoise = PerlinNoiseBiome.CreateInstance<PerlinNoiseBiome>();
        _voronoi = VoronoiBiome.CreateInstance<VoronoiBiome>();
    }

    private void ResetInstances()
    {
        MapBuilder.Instance.CurrentBlueprint = _bp = Object.Instantiate(_recipeBp);
    }
     
    [Test]
    public void Algorithm_In_Stack()
    {
        ResetInstances();

        AlgorithmStorage storage = new AlgorithmStorage(_spelunky);
        _bp.AlgorithmStack.Add(storage);

        Assert.AreEqual(storage.Algorithm, _spelunky);
    }

    [Test]
    public void ChunkPlacer_Execution()
    {
        ResetInstances();

        AlgorithmStorage spelunkyStorage = new AlgorithmStorage(_spelunky);

        _bp.AlgorithmStack.Add(spelunkyStorage);

        bool result = MapBuilder.Instance.Generate(_bp);

        Assert.IsTrue(result);
    }

    [Test]
    public void Spelunky_Algorithm_Execution()
    {
        ResetInstances();

        AlgorithmStorage storage = new AlgorithmStorage(_spelunky);
        _bp.AlgorithmStack.Add(storage);

        MapBuilder.Instance.Generate(_bp);

        Assert.IsTrue(MapBuilder.Instance.ActiveMap);
    }

    [Test]
    public void DrunkardWalk_Algorithm_Execution()
    {
        ResetInstances();

        AlgorithmStorage storage = new AlgorithmStorage(_drunkard);
        _bp.AlgorithmStack.Add(storage);

        MapBuilder.Instance.Generate(_bp);

        Assert.IsTrue(MapBuilder.Instance.ActiveMap);
    }

    [Test]
    public void DeadEndMaker_Algorithm()
    {
        ResetInstances();

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
        ResetInstances();

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
        ResetInstances();

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
        ResetInstances();

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
