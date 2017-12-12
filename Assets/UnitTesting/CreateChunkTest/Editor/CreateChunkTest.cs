using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using MapGeneration;

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