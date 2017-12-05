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
            return !l.Any() ? default(T) : l[r.Range(0, l.Count)];
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

        /// <summary>
        /// Returns a randon item from list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="l">List</param>
        /// <param name="predicate">The predicate used in the filtration of the list.</param>
        /// <returns></returns>
        public static T RandomEntry<T>(this IList<T> l, Func<T, bool> predicate)
        {
            return l.Where(predicate).ToList().RandomEntry();
        }

        /// <summary>
        /// Returns a randon item from list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="l">List</param>
        /// <param name="predicate">The predicate used in the filtration of the list.</param>
        /// <param name="r">Random instance</param>
        /// <returns></returns>
        public static T RandomEntry<T>(this IList<T> l, Func<T, bool> predicate, Random r)
        {
            return l.Where(predicate).ToList().RandomEntry(r);
        }
    }
}