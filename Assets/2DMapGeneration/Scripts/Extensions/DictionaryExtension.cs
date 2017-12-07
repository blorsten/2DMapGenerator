using System.Collections.Generic;

namespace MapGeneration.Extensions
{
    public static class DictionaryExtension
    {
        /// <summary>
        /// Gets a value from a key value if it doesnt exist, create the entry.
        /// </summary>
        /// <typeparam name="T1">Type of the key</typeparam>
        /// <typeparam name="T2">Type of the value</typeparam>
        /// <param name="dictionary">Dictionary to operate on.</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
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