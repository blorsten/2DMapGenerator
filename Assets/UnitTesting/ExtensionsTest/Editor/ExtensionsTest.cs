using System;
using System.Collections.Generic;
using MapGeneration;
using MapGeneration.Algorithm;
using MapGeneration.ChunkSystem;
using MapGeneration.Extensions;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

[TestFixture(Author = "MP", Category = "Extensions")]
public class ExtensionTest
{
    #region Chunk Extension Compare Size Two Chunks

    [Test]
    public void Chunk_Extension_Compare_Size_Two_Chunks_Same_Size()
    {
        Chunk firstChunk = new GameObject().AddComponent<Chunk>();
        firstChunk.Width = 10;
        firstChunk.Height = 10;

        Chunk secondChunk = new GameObject().AddComponent<Chunk>();
        secondChunk.Width = 10;
        secondChunk.Height = 10;

        Assert.IsTrue(firstChunk.CompareSize(secondChunk));
    }

    [Test]
    public void Chunk_Extension_Compare_Size_Two_Chunks_Not_Same_Size()
    {
        Chunk firstChunk = new GameObject().AddComponent<Chunk>();
        firstChunk.Width = 5;
        firstChunk.Height = 5;

        Chunk secondChunk = new GameObject().AddComponent<Chunk>();
        secondChunk.Width = 10;
        secondChunk.Height = 10;

        Assert.IsFalse(firstChunk.CompareSize(secondChunk));
    }

    [Test]
    public void Chunk_Extension_Compare_Size_Two_Chunks_Same_Width_Not_Same_Height()
    {
        Chunk firstChunk = new GameObject().AddComponent<Chunk>();
        firstChunk.Width = 5;
        firstChunk.Height = 5;

        Chunk secondChunk = new GameObject().AddComponent<Chunk>();
        secondChunk.Width = 5;
        secondChunk.Height = 10;

        Assert.IsFalse(firstChunk.CompareSize(secondChunk));
    }

    [Test]
    public void Chunk_Extension_Compare_Size_Two_Chunks_Same_Height_Not_Same_Width()
    {
        Chunk firstChunk = new GameObject().AddComponent<Chunk>();
        firstChunk.Width = 10;
        firstChunk.Height = 10;

        Chunk secondChunk = new GameObject().AddComponent<Chunk>();
        secondChunk.Width = 5;
        secondChunk.Height = 10;

        Assert.IsFalse(firstChunk.CompareSize(secondChunk));
    }

    #endregion

    #region Chunk Extension Compare Size With Int

    [Test]
    public void Chunk_Extension_Compare_Size_Int_Same_Size()
    {
        Chunk firstChunk = new GameObject().AddComponent<Chunk>();
        firstChunk.Width = 10;
        firstChunk.Height = 10;

        Assert.IsTrue(firstChunk.CompareSize(10, 10));
    }

    [Test]
    public void Chunk_Extension_Compare_Size_Int_Not_Same_Size()
    {
        Chunk firstChunk = new GameObject().AddComponent<Chunk>();
        firstChunk.Width = 5;
        firstChunk.Height = 5;

        Assert.IsFalse(firstChunk.CompareSize(10, 10));
    }

    [Test]
    public void Chunk_Extension_Compare_Size_Int_Same_Width_Not_Same_Height()
    {
        Chunk firstChunk = new GameObject().AddComponent<Chunk>();
        firstChunk.Width = 5;
        firstChunk.Height = 5;

        Assert.IsFalse(firstChunk.CompareSize(5, 10));
    }

    [Test]
    public void Chunk_Extension_Compare_Size_Int_Same_Height_Not_Same_Width()
    {
        Chunk firstChunk = new GameObject().AddComponent<Chunk>();
        firstChunk.Width = 10;
        firstChunk.Height = 10;

        Assert.IsFalse(firstChunk.CompareSize(5, 10));
    }

    #endregion

    #region Chunk Extension Compare Size With Vector2Int

    [Test]
    public void Chunk_Extension_Compare_Size_Vector2Int_Same_Size()
    {
        Chunk firstChunk = new GameObject().AddComponent<Chunk>();
        firstChunk.Width = 10;
        firstChunk.Height = 10;

        Assert.IsTrue(firstChunk.CompareSize(new Vector2Int(10, 10)));
    }

    [Test]
    public void Chunk_Extension_Compare_Size_Vector2Int_Not_Same_Size()
    {
        Chunk firstChunk = new GameObject().AddComponent<Chunk>();
        firstChunk.Width = 5;
        firstChunk.Height = 5;

        Assert.IsFalse(firstChunk.CompareSize(new Vector2Int(10, 10)));
    }

    [Test]
    public void Chunk_Extension_Compare_Size_Vector2Int_Same_Width_Not_Same_Height()
    {
        Chunk firstChunk = new GameObject().AddComponent<Chunk>();
        firstChunk.Width = 5;
        firstChunk.Height = 5;

        Assert.IsFalse(firstChunk.CompareSize(new Vector2Int(5, 10)));
    }

    [Test]
    public void Chunk_Extension_Compare_Size_Vector2Int_Same_Height_Not_Same_Width()
    {
        Chunk firstChunk = new GameObject().AddComponent<Chunk>();
        firstChunk.Width = 10;
        firstChunk.Height = 10;

        Assert.IsFalse(firstChunk.CompareSize(new Vector2Int(5, 10)));
    }

    #endregion

    #region Dictionary Extension
    [Test]
    public void Dictionary_Extension_Get_Or_Add_Getted()
    {
        Dictionary<int, string> dictionary = new Dictionary<int, string>
        {
            {1, "test1"},
            {2, "test2"},
            {3, "test3"}
        };

        dictionary.GetOrAdd(3, "test3");
        Assert.IsTrue(dictionary.Count == 3);
    }

    [Test]
    public void Dictionary_Extension_Get_Or_Add_Added()
    {
        Dictionary<int, string> dictionary = new Dictionary<int, string>
        {
            {1, "test1"},
            {2, "test2"},
            {3, "test3"}
        };

        dictionary.GetOrAdd(4, "test3");
        Assert.IsTrue(dictionary.Count == 4);
        Assert.IsTrue(dictionary.ContainsKey(4));
    }
    #endregion

    #region List Extension Predined Random
    [Test]
    public void List_Extension_Random_Entry_Predefined_Random()
    {
        List<int> newList = new List<int>
        {
            1,
            2,
            3
        };

        Assert.AreEqual(2, newList.RandomEntry(new System.Random(1)));
    }

    [Test]
    public void List_Extension_Random_Entry_Predefined_Random_With_Predicate()
    {
        List<int> newList = new List<int>
        {
            1,
            2,
            3
        };

        Assert.AreEqual(2, newList.RandomEntry(i => i >= 2,new System.Random(1)));
    }
    #endregion

    #region Random Extensions
    [Test]
    public void Random_Extension_Generate_Byte_Seed_Predined_Random()
    {
        System.Random newRandom = new System.Random(1);

        byte[] expectedSeed =
        {
            93,
            53,
            244,
            64,
            232,
            93,
            192,
            154,
            214,
            91,
            219,
            140,
            166,
            196,
            172,
            251
        };

        Assert.AreEqual(expectedSeed, RandomExtension.GenerateByteSeed(newRandom));
    }

    [Test]
    public void Random_Extension_Range_Int()
    {
        System.Random newRandom = new System.Random(1);
        Assert.AreEqual(0, newRandom.Range(0, 2));
    }

    [Test]
    public void Random_Extension_Range_Float()
    {
        System.Random newRandom = new System.Random(1);
        Assert.AreEqual(2.2337091f, newRandom.Range(1.5f, 3.5f));
    }

    [Test]
    public void Random_Extension_Range_Vector2()
    {
        System.Random newRandom = new System.Random(1);
        Vector2 randomVector2 = newRandom.Range(new Vector2(1, 1), new Vector2(5, 5));
        Assert.AreEqual(2.46741843f, randomVector2.x);
        Assert.AreEqual(1.83173895f, randomVector2.y);
    }

    [Test]
    public void Random_Extension_Range_Vector2Int()
    {
        System.Random newRandom = new System.Random(1);
        Assert.AreEqual(new Vector2Int(2, 1), newRandom.Range(new Vector2Int(1, 1), new Vector2Int(5, 5)));
    }
    #endregion
     
    [Test]
    public void Type_Extension_Get_Default_Value()
    {
        Assert.AreEqual(default(int), typeof(int).GetDefaultValue());
        Assert.AreEqual(default(float), typeof(float).GetDefaultValue());
        Assert.AreEqual(null, typeof(ChunkOpenings).GetDefaultValue());
        Assert.AreEqual(new AlgorithmStorage(null) {IsActive = false}, typeof(AlgorithmStorage).GetDefaultValue());
    }
}
