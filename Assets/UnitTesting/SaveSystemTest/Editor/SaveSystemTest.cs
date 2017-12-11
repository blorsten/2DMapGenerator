using System;
using System.Collections;
using MapGeneration;
using MapGeneration.SaveSystem;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.TestTools;

[TestFixture(Author = "MP", Category = "SaveSystem")]
public class SaveSystemTest
{
    public SaveSystemTest()
    {
        MapBuilder.Instance.CurrentBlueprint =
            AssetDatabase.LoadAssetAtPath<MapBlueprint>("Assets/UnitTesting/SaveSystemTest/SaveSystemBPTest.asset");
    }

    [Test]
    public void MapDataSaver_Construction_And_Initalization()
    {
        Map newMap = MapBuilder.Instance.Generate();

        Assert.AreEqual(newMap, newMap.MapDataSaver.Map);
        Assert.AreEqual(newMap.ID, newMap.MapDataSaver.MapId);
        Assert.AreEqual(newMap.Seed, newMap.MapDataSaver.MapSeed);
        Assert.AreEqual(newMap.MapBlueprint, newMap.MapDataSaver.MapBlueprint);
    }

    [Test]
    public void MapDataSaver_Save()
    {
        Map newMap = MapBuilder.Instance.Generate();

        newMap.GetComponentInChildren<SaveSystemTestScript>().TestInt = Int32.MaxValue; 

        newMap.MapDataSaver.SavePersistentData();

        Assert.IsTrue(newMap.MapDataSaver.HasSavedData);
    }

    [Test]
    public void MapDataSaver_Load_With_Generation()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestInt = Int32.MaxValue;

        Map secondMap = MapBuilder.Instance.Generate(newMap);

        Assert.AreEqual(Int32.MaxValue, secondMap.GetComponentInChildren<SaveSystemTestScript>().TestInt);
    }

    [Test]
    public void MapDataSaver_Save_Load_Int()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestInt = Int32.MaxValue;

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual(Int32.MaxValue, newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestInt);
    }

    [Test]
    public void MapDataSaver_Save_Load_Prop_Int()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestIntProp = Int32.MaxValue;

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual(Int32.MaxValue, newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestIntProp);
    }

    [Test]
    public void MapDataSaver_Save_Load_String()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestString = "teststring";

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual("teststring", newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestString);
    }

    [Test]
    public void MapDataSaver_Save_Load_Prop_String()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestStringProp = "teststring";

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual("teststring", newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestStringProp);
    }

    [Test]
    public void MapDataSaver_Save_Load_Float()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestFloat = Single.MaxValue;

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual(Single.MaxValue, newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestFloat);
    }

    [Test]
    public void MapDataSaver_Save_Load_Prop_Float()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestFloatProp = Single.MaxValue;

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual(Single.MaxValue, newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestFloatProp);
    }

    [Test]
    public void MapDataSaver_Save_Load_Double()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestDouble = Double.MaxValue;

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual(Double.MaxValue, newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestDouble);
    }

    [Test]
    public void MapDataSaver_Save_Load_Prop_Double()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestDoubleProp = Double.MaxValue;

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual(Double.MaxValue, newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestDoubleProp);
    }

    [Test]
    public void MapDataSaver_Save_Load_Byte()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestByte = Byte.MaxValue;

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual(Byte.MaxValue, newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestByte);
    }

    [Test]
    public void MapDataSaver_Save_Load_Prop_Byte()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestByteProp = Byte.MaxValue;

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual(Byte.MaxValue, newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestByteProp);
    }

    [Test]
    public void MapDataSaver_Save_Load_Char()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestChar = Char.MaxValue;

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual(Char.MaxValue, newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestChar);
    }

    [Test]
    public void MapDataSaver_Save_Load_Prop_Char()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestCharProp = Char.MaxValue;

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual(Char.MaxValue, newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestCharProp);
    }

    [Test]
    public void MapDataSaver_Save_Load_Decimal()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestDecimal = Decimal.MaxValue;

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual(Decimal.MaxValue, newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestDecimal);
    }

    [Test]
    public void MapDataSaver_Save_Load_Prop_Decimal()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestDecimalProp = Decimal.MaxValue;

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual(Decimal.MaxValue, newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestDecimalProp);
    }

    [Test]
    public void MapDataSaver_Save_Load_Long()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestLong = Int64.MaxValue;

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual(Int64.MaxValue, newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestLong);
    }

    [Test]
    public void MapDataSaver_Save_Load_Prop_Long()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestLongProp = Int64.MaxValue;

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual(Int64.MaxValue, newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestLongProp);
    }

    [Test]
    public void MapDataSaver_Save_Load_SByte()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestSByte = SByte.MaxValue;

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual(SByte.MaxValue, newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestSByte);
    }

    [Test]
    public void MapDataSaver_Save_Load_Prop_SByte()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestSByteProp = SByte.MaxValue;

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual(SByte.MaxValue, newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestSByteProp);
    }

    [Test]
    public void MapDataSaver_Save_Load_Short()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestShort = Int16.MaxValue;

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual(Int16.MaxValue, newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestShort);
    }

    [Test]
    public void MapDataSaver_Save_Load_Prop_Short()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestShortProp = Int16.MaxValue;

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual(Int16.MaxValue, newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestShortProp);
    }

    [Test]
    public void MapDataSaver_Save_Load_UInt()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestUInt = UInt32.MaxValue;

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual(UInt32.MaxValue, newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestUInt);
    }

    [Test]
    public void MapDataSaver_Save_Load_Prop_UInt()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestUIntProp = UInt32.MaxValue;

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual(UInt32.MaxValue, newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestUIntProp);
    }

    [Test]
    public void MapDataSaver_Save_Load_ULong()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestULong = UInt64.MaxValue;

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual(UInt64.MaxValue, newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestULong);
    }

    [Test]
    public void MapDataSaver_Save_Load_Prop_ULong()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestULongProp = UInt64.MaxValue;

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual(UInt64.MaxValue, newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestULongProp);
    }

    [Test]
    public void MapDataSaver_Save_Load_UShort()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestUShort = UInt16.MaxValue;

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual(UInt16.MaxValue, newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestUShort);
    }

    [Test]
    public void MapDataSaver_Save_Load_Prop_UShort()
    {
        MapDataSaver newMap = MapBuilder.Instance.Generate().MapDataSaver;

        newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestUShortProp = UInt16.MaxValue;

        Assert.IsFalse(newMap.HasSavedData);

        newMap.SavePersistentData();

        Assert.IsTrue(newMap.HasSavedData);

        newMap.LoadPersistentData();

        Assert.AreEqual(UInt16.MaxValue, newMap.Map.GetComponentInChildren<SaveSystemTestScript>().TestUShortProp);
    }
}
