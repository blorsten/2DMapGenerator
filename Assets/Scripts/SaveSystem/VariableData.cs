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
        //The type of class this data belongs to do.
        public Type Type { get; private set; }

        //The kind of data either property or field.
        public T Info { get; private set; }

        //DataIdentity ID that this data belongs to.
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