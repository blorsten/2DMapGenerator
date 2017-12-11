 using System;
using System.Reflection;

namespace MapGeneration.SaveSystem
{
    /// <summary>
    /// This Struct is used to save down persistent data, used in <see cref="MapDataSaver"/>
    /// </summary>
    public struct VariableData<T> where T : MemberInfo
    {
        /// <summary>
        /// The type of class this data belongs to do.
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// The kind of data either property or field.
        /// </summary>
        public T Info { get; private set; }

        /// <summary>
        /// DataIdentity ID that this data belongs to.
        /// </summary>
        public Guid Id { get; private set; }
    
        /// <summary>
        /// The actual data.
        /// </summary>
        public object Data { get; private set; }

        /// <summary>
        /// Constructs the Struct and sets it values.
        /// </summary>
        /// <param name="type">"type"<see cref="Type"</param>
        /// <param name="info">"type"<see cref="Info"</param>
        /// <param name="id">"type"<see cref="Id"</param>
        /// <param name="data">"type"<see cref="Data"</param>
        public VariableData(Type type, T info, Guid id, object data) : this()
        {
            Type = type;
            Info = info;
            Id = id;
            Data = data;
        }
    }
}