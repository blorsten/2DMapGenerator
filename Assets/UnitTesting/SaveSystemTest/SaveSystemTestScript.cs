using MapGeneration.SaveSystem;
using UnityEngine;

public class SaveSystemTestScript : MonoBehaviour
{
    [SerializeField, PersistentData]
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
#pragma warning disable 169
    public int TestInt;

#pragma warning restore 169

    [PersistentData]
    public int TestIntProp { get; set; }

    [SerializeField, PersistentData]
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
#pragma warning disable 169
    public string TestString;

#pragma warning restore 169

    [PersistentData]
    public string TestStringProp { get; set; }

    [SerializeField, PersistentData]
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
#pragma warning disable 169
    public float TestFloat;

#pragma warning restore 169

    [PersistentData]
    public float TestFloatProp { get; set; }

    [SerializeField, PersistentData]
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
#pragma warning disable 169
    public double TestDouble;

#pragma warning restore 169

    [PersistentData]
    public double TestDoubleProp { get; set; }

    [SerializeField, PersistentData]
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
#pragma warning disable 169
    public byte TestByte;

#pragma warning restore 169

    [PersistentData]
    public byte TestByteProp { get; set; }

    [SerializeField, PersistentData]
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
#pragma warning disable 169
    public char TestChar;

#pragma warning restore 169

    [PersistentData]
    public char TestCharProp { get; set; }

    [SerializeField, PersistentData]
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
#pragma warning disable 169
    public decimal TestDecimal;

#pragma warning restore 169

    [PersistentData]
    public decimal TestDecimalProp { get; set; }

    [SerializeField, PersistentData]
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
#pragma warning disable 169
    public long TestLong;

#pragma warning restore 169

    [PersistentData]
    public long TestLongProp { get; set; }

    [SerializeField, PersistentData]
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
#pragma warning disable 169
    public sbyte TestSByte;

#pragma warning restore 169

    [PersistentData]
    public sbyte TestSByteProp { get; set; }

    [SerializeField, PersistentData]
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
#pragma warning disable 169
    public short TestShort;

#pragma warning restore 169

    [PersistentData]
    public short TestShortProp { get; set; }

    [SerializeField, PersistentData]
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
#pragma warning disable 169
    public uint TestUInt;

#pragma warning restore 169

    [PersistentData]
    public uint TestUIntProp { get; set; }

    [SerializeField, PersistentData]
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
#pragma warning disable 169
    public ulong TestULong;

#pragma warning restore 169

    [PersistentData]
    public ulong TestULongProp { get; set; }

    [SerializeField, PersistentData]
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
#pragma warning disable 169
    public ushort TestUShort;

#pragma warning restore 169

    [PersistentData]
    public ushort TestUShortProp { get; set; }
}
