using UnityEngine;
using NUnit.Framework;
using System.Linq;
using MapGeneration.ChunkSystem;
using MapGeneration.Utils;

[TestFixture(Author = "MP", Category = "Utils")]
public class UtilitiesTest
{
    [Test]
    public void ChunkHolder_2D_Array_Instantiation()
    {
        ChunkHolder2DArray newArray = new ChunkHolder2DArray(2,2);
        Assert.IsNotNull(newArray);
    }

    [Test]
    public void ChunkHolder_2D_Array_Assignment()
    {
        ChunkHolder2DArray newArray = new ChunkHolder2DArray(2, 2);

        newArray[0, 0] = new ChunkHolder(new Vector2Int(0, 0));
        newArray[1, 0] = new ChunkHolder(new Vector2Int(1, 0));
        newArray[0, 1] = new ChunkHolder(new Vector2Int(0, 1));
        newArray[1, 1] = new ChunkHolder(new Vector2Int(1, 1));

        Assert.AreEqual(4, newArray.Count());
        Assert.AreEqual(new Vector2Int(0, 0), newArray[0, 0].Position);
        Assert.AreEqual(new Vector2Int(1, 0), newArray[1, 0].Position);
        Assert.AreEqual(new Vector2Int(0, 1), newArray[0, 1].Position);
        Assert.AreEqual(new Vector2Int(1, 1), newArray[1, 1].Position);
    }

    [Test]
    public void ChunkHolder_2D_Array_Length()
    {
        ChunkHolder2DArray newArray = new ChunkHolder2DArray(5, 7);
        Assert.AreEqual(5, newArray.GetLength(0));
        Assert.AreEqual(7, newArray.GetLength(1));
    }

    [Test]
    public void ChunkHolder_2D_Array_Enumeration()
    {
        ChunkHolder2DArray newArray = new ChunkHolder2DArray(2, 2);

        newArray[0, 0] = new ChunkHolder(new Vector2Int(1, 1));
        newArray[1, 0] = new ChunkHolder(new Vector2Int(1, 1));
        newArray[0, 1] = new ChunkHolder(new Vector2Int(1, 1));
        newArray[1, 1] = new ChunkHolder(new Vector2Int(1, 1));

        foreach (ChunkHolder chunkHolder in newArray)
        {
            Assert.NotNull(chunkHolder);
            Assert.AreEqual(new Vector2Int(1, 1), chunkHolder.Position);
        }
    }

    [Test]
    public void Float_2D_Array_Instantiation()
    {
        Float2DArray newArray = new Float2DArray(2, 2);
        Assert.IsNotNull(newArray);
    }

    [Test]
    public void Float_2D_Array_Assignment()
    {
        Float2DArray newArray = new Float2DArray(2, 2);

        newArray[0, 0] = 1.11f;
        newArray[1, 0] = 2.22f;
        newArray[0, 1] = 3.33f;
        newArray[1, 1] = 4.44f;

        Assert.AreEqual(4, newArray.Count());
        Assert.AreEqual(1.11f, newArray[0, 0]);
        Assert.AreEqual(2.22f, newArray[1, 0]);
        Assert.AreEqual(3.33f, newArray[0, 1]);
        Assert.AreEqual(4.44f, newArray[1, 1]);
    }

    [Test]
    public void Float_2D_Array_Length()
    {
        Float2DArray newArray = new Float2DArray(3, 5);
        Assert.AreEqual(3, newArray.GetLength(0));
        Assert.AreEqual(5, newArray.GetLength(1));
    }

    [Test]
    public void Float_2D_Array_Enumeration()
    {
        Float2DArray newArray = new Float2DArray(2, 2);

        newArray[0, 0] = 1.11f;
        newArray[1, 0] = 1.11f;
        newArray[0, 1] = 1.11f;
        newArray[1, 1] = 1.11f;

        foreach (float chunkHolder in newArray)
        {
            Assert.AreEqual(1.11f, chunkHolder);
        }
    }

    [Test]
    public void Int_2D_Array_Instantiation()
    {
        Int2DArray newArray = new Int2DArray(2, 2);
        Assert.IsNotNull(newArray);
    }

    [Test]
    public void Int_2D_Array_Assignment()
    {
        Int2DArray newArray = new Int2DArray(2, 2);

        newArray[0, 0] = 1;
        newArray[1, 0] = 2;
        newArray[0, 1] = 3;
        newArray[1, 1] = 4;

        Assert.AreEqual(4, newArray.Count());
        Assert.AreEqual(1, newArray[0, 0]);
        Assert.AreEqual(2, newArray[1, 0]);
        Assert.AreEqual(3, newArray[0, 1]);
        Assert.AreEqual(4, newArray[1, 1]);
    }

    [Test]
    public void Int_2D_Array_Length()
    {
        Int2DArray newArray = new Int2DArray(3, 5);
        Assert.AreEqual(3, newArray.GetLength(0));
        Assert.AreEqual(5, newArray.GetLength(1));
    }

    [Test]
    public void Int_2D_Array_Enumeration()
    {
        Int2DArray newArray = new Int2DArray(2, 2);

        newArray[0, 0] = 1;
        newArray[1, 0] = 1;
        newArray[0, 1] = 1;
        newArray[1, 1] = 1;

        foreach (int chunkHolder in newArray)
        {
            Assert.AreEqual(1, chunkHolder);
        }
    }

    [Test]
    public void GameObject_Utilities()
    {
        GameObject father = new GameObject("Test Object");

        new GameObject("Child 1").transform.SetParent(father.transform);
        new GameObject("Child 2").transform.SetParent(father.transform);
        new GameObject("Child 3").transform.SetParent(father.transform);
        new GameObject("Child 4").transform.SetParent(father.transform);
        new GameObject("Child 5").transform.SetParent(father.transform);

        Assert.IsTrue(father.transform.childCount == 5);

        GameObjectUtils.DestroyChildren(father.gameObject, true);
         
        Assert.IsTrue(father.transform.childCount == 0);
    }

    [Test]
    public void Singleton_Usage()
    {
        Assert.IsNotNull(SingletonTest.Instance);
        SingletonTest.Instance.TestInt = 100;
        Assert.AreEqual(100, SingletonTest.Instance.TestInt);
        Object.DestroyImmediate(SingletonTest.Instance.gameObject);
    }

    [Test]
    public void Singleton_Check_Sanity()
    {
        Assert.IsFalse(SingletonTest.CheckSanity());
        Assert.IsNotNull(SingletonTest.Instance);
        Assert.IsTrue(SingletonTest.CheckSanity());
    }
}
