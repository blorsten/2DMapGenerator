using System.Collections;
using MapGeneration;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class CreateChunkTest
{
    [Test]
    public void Create_Chunk_Test()
    {
        CreateChunk.CreateDefaultChunk();

        GameObject newChunk = GameObject.Find("New Chunk(Clone)");

        Assert.NotNull(newChunk);
    }
}