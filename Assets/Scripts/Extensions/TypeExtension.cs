using System;
using System.Collections.Generic;

namespace MapGeneration.Extensions
{
    public static class TypeExtension
    {
        //a thread-safe way to hold default instances created at run-time
        private static readonly Dictionary<Type, object> TypeDefaults =
            new Dictionary<Type, object>();

        public static object GetDefaultValue(this Type type)
        {
            return type.IsValueType
                ? TypeDefaults.GetOrAdd(type, Activator.CreateInstance(type))
                : null;
        }
    }
}