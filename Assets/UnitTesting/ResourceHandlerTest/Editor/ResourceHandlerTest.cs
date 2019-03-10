using System.Collections;
using System.Collections.Generic;
using MapGeneration;
using MapGeneration.ChunkSystem;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture(Author = "PW", Category = "ResourceHandler")]
public class ResourceHandlerTest
{
    [Test]
    public void Load_Chunks()
    {
        List<Chunk> loadedChunks = new List<Chunk>();
        loadedChunks.AddRange(Resources.LoadAll<Chunk>(string.Empty));

        ResourceHandler.Instance.UpdateChunks();

        CollectionAssert.AreEqual(loadedChunks, ResourceHandler.Instance.Chunks);
    }

    [Test]
    public void Load_Objects()
    {
        List<GameplayObject> loadedObjects = new List<GameplayObject>();
        loadedObjects.AddRange(Resources.LoadAll<GameplayObject>(string.Empty));

        ResourceHandler.Instance.UpdateObjects();

        CollectionAssert.AreEqual(loadedObjects, ResourceHandler.Instance.Objects);
    }

    [Test]
    public void Loaded_Correct_Chunks()
    {
        ResourceHandler.Instance.UpdateChunks();

        CollectionAssert.AllItemsAreInstancesOfType(
            ResourceHandler.Instance.Chunks,
            typeof(Chunk));
    }

    [Test]
    public void Loaded_Correct_Objects()
    {
        ResourceHandler.Instance.UpdateObjects();

        CollectionAssert.AllItemsAreInstancesOfType(
            ResourceHandler.Instance.Objects,
            typeof(GameplayObject));
    }

    [Test]
    public void Did_Not_Load_Chunks_Duplicate()
    {
        ResourceHandler.Instance.UpdateChunks();

        CollectionAssert.AllItemsAreUnique(ResourceHandler.Instance.Chunks);
    }

    [Test]
    public void Did_Not_Load_Ojects_Duplicate()
    {
        ResourceHandler.Instance.UpdateObjects();

        CollectionAssert.AllItemsAreUnique(ResourceHandler.Instance.Objects);
    }

    [Test]
    public void Set_Loaded_Chunks()
    {
        ResourceHandler.Instance.UpdateChunks();

        Assert.IsInstanceOf<List<Chunk>>(ResourceHandler.Instance.Chunks);
    }

    [Test]
    public void Set_Loaded_Objects()
    {
        ResourceHandler.Instance.UpdateObjects();

        Assert.IsInstanceOf<List<GameplayObject>>(ResourceHandler.Instance.Objects);
    }
}