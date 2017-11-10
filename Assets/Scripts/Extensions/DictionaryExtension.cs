using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration.Extensions
{
    public static class DictionaryExtension
    {
        public static T2 GetOrAdd<T1, T2>(this Dictionary<T1, T2> dictionary, T1 key, T2 value)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            dictionary.Add(key, value);
            return value;
        }
    }
}