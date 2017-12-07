using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MapGeneration.Extensions
{
    public static class AssetDatabaseExtension
    {
        /// <summary>
        /// Tries to find all assets of the specific type
        /// </summary>
        /// <typeparam name="T">Type of asset</typeparam>
        /// <returns>List of all the assets found.</returns>
        public static List<T> FindAssetsByType<T>() where T : Object
        {
            List<T> assets = new List<T>();

            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

                if (asset != null)
                    assets.Add(asset);
            }

            return assets;
        }
    }
}