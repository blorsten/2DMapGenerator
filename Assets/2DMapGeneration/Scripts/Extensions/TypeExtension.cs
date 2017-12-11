using System;
using System.Collections.Generic;

namespace MapGeneration.Extensions
{
    /// <summary>
    /// An extension for the .net class Type.
    /// </summary>
    public static class TypeExtension
    {
        //A thread-safe way to hold default instances created at run-time.
        private static readonly Dictionary<Type, object> TypeDefaults =
            new Dictionary<Type, object>();

        /// <summary>
        /// Tries to get a saved default value from a type, if it doesn't exist create it.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetDefaultValue(this Type type)
        {
            return type.IsValueType
                ? TypeDefaults.GetOrAdd(type, Activator.CreateInstance(type))
                : null;
        }
    }
}