using MapGeneration.Utils;
using UnityEngine;

public class SingletonTest : Singleton<SingletonTest>
{
    public int TestInt { get; set; }
}