using System;

namespace MapGeneration.SaveSystem
{
    /// <summary>
    /// This class is used to mark variables that needs saving.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class PersistentData : Attribute
    { }
}