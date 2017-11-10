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
        public Type Type { get; private set; }
        public T Info { get; private set; }
        public Guid Id { get; private set; }
        public object Data { get; private set; }

        public VariableData(Type type, T info, Guid id, object data) : this()
        {
            Type = type;
            Info = info;
            Id = id;
            Data = data;
        }
    }
}