using System;
using System.Reflection;

namespace MapGeneration.SaveSystem
{
    /// <summary>
    /// Purpose: Struct used to save down persistent data, used in <see cref="MapDataSaver"/>
    /// Creator: MP
    /// </summary>
    public struct VariableData<T> where T : MemberInfo
    {
        public Type Type;
        public T Info;
        public Guid Id;
        public object Data;

        public VariableData(Type type, T info, Guid id, object data)
        {
            Type = type;
            Info = info;
            Id = id;
            Data = data;
        }
    }
}