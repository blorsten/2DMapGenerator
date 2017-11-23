using System.Collections.Generic;
using System;
using System.Linq;
using MapGeneration.Extensions;

namespace ListExstention
{
    /// <summary>
    /// Purpose:
    /// Creator:
    /// </summary>
    public static class ListExstention 
    {
        /// <summary>
        /// Returns a random entry in list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="l">List</param>
        /// <param name="r">Random instance</param>
        /// <returns>item from list</returns>
        public static T RandomEntry<T>(this IList<T> l, Random r)
        {
            return l[r.Range(0, l.Count)];
        }

        /// <summary>
        /// Returns a randon item from list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="l">List</param>
        /// <returns>Random item from list</returns>
        public static T RandomEntry<T>(this IList<T> l)
        {
            return RandomEntry(l, new Random());
        }

        public static T RandomEntry<T>(this IList<T> l, Func<T, bool> predicate)
        {
            return l.Where(predicate).ToList().RandomEntry();
        }

        public static T RandomEntry<T>(this IList<T> l, Func<T, bool> predicate, Random r)
        {
            return l.Where(predicate).ToList().RandomEntry(r);
        }
    }
}