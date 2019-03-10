using MapGeneration.SaveSystem;
using UnityEngine;

public class SaveSystemTestScript : MonoBehaviour
{
    [SerializeField, PersistentData]
    public int TestInt;

    [PersistentData]
    public int TestIntProp { get; set; }

    [SerializeField, PersistentData]
    public string TestString;

    [PersistentData]
    public string TestStringProp { get; set; }

    [SerializeField, PersistentData]
    public float TestFloat;

    [PersistentData]
    public float TestFloatProp { get; set; }

    [SerializeField, PersistentData]
    public double TestDouble;

    [PersistentData]
    public double TestDoubleProp { get; set; }

    [SerializeField, PersistentData]
    public byte TestByte;

    [PersistentData]
    public byte TestByteProp { get; set; }

    [SerializeField, PersistentData]
    public char TestChar;

    [PersistentData]
    public char TestCharProp { get; set; }

    [SerializeField, PersistentData]
    public decimal TestDecimal;

    [PersistentData]
    public decimal TestDecimalProp { get; set; }

    [SerializeField, PersistentData]
    public long TestLong;

    [PersistentData]
    public long TestLongProp { get; set; }

    [SerializeField, PersistentData]
    public sbyte TestSByte;

    [PersistentData]
    public sbyte TestSByteProp { get; set; }

    [SerializeField, PersistentData]
    public short TestShort;

    [PersistentData]
    public short TestShortProp { get; set; }

    [SerializeField, PersistentData]
    public uint TestUInt;

    [PersistentData]
    public uint TestUIntProp { get; set; }

    [SerializeField, PersistentData]
    public ulong TestULong;

    [PersistentData]
    public ulong TestULongProp { get; set; }

    [SerializeField, PersistentData]
    public ushort TestUShort;

    [PersistentData]
    public ushort TestUShortProp { get; set; }
}