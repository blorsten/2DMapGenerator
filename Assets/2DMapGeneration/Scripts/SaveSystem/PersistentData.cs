using System;

namespace MapGeneration.SaveSystem
{
    /// <summary>
    /// Purpose: Used to mark variables that needs saving.
    /// Creator: MP
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class PersistentData : Attribute
    {
    }
}