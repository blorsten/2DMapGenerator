using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using MapGeneration.ChunkSystem;
using MapGeneration;

[TestFixture(Author = "PW", Category = "ResourceHandler")]
public class ResourceHandlerTest
{
    [Test]
    public void LoadChunks()
    {
        List<Chunk> loadedChunks = new List<Chunk>();
        loadedChunks.AddRange(Resources.LoadAll<Chunk>(string.Empty));

        ResourceHandler.Instance.UpdateChunks();

        CollectionAssert.AreEqual(loadedChunks, ResourceHandler.Instance.Chunks);
    }

    [Test]
    public void LoadObjects()
    {
        List<GameplayObject> loadedObjects = new List<GameplayObject>();
        loadedObjects.AddRange(Resources.LoadAll<GameplayObject>(string.Empty));

        ResourceHandler.Instance.UpdateObjects();

        CollectionAssert.AreEqual(loadedObjects, ResourceHandler.Instance.Objects);
    }

    [Test]
    public void LoadedCorrecChunks()
    {
        ResourceHandler.Instance.UpdateChunks();

        CollectionAssert.AllItemsAreInstancesOfType(
            ResourceHandler.Instance.Chunks,
            typeof(Chunk));
    }

    [Test]
    public void LoadedCorrectObjects()
    {
        ResourceHandler.Instance.UpdateObjects();

        CollectionAssert.AllItemsAreInstancesOfType(
            ResourceHandler.Instance.Objects,
            typeof(GameplayObject));
    }

    [Test]
    public void DidNotLoadChunksDuplicates()
    {
        ResourceHandler.Instance.UpdateChunks();

        CollectionAssert.AllItemsAreUnique(ResourceHandler.Instance.Chunks);
    }

    [Test]
    public void DidNotLoadOjectsDuplicates()
    {
        ResourceHandler.Instance.UpdateObjects();

        CollectionAssert.AllItemsAreUnique(ResourceHandler.Instance.Objects);
    }

    [Test]
    public void SetLoadedChunks()
    {
        ResourceHandler.Instance.UpdateChunks();

        Assert.IsInstanceOf<List<Chunk>>(ResourceHandler.Instance.Chunks);
    }

    [Test]
    public void SetLoadedObjects()
    {
        ResourceHandler.Instance.UpdateObjects();

        Assert.IsInstanceOf<List<GameplayObject>>(ResourceHandler.Instance.Objects);
    }
}