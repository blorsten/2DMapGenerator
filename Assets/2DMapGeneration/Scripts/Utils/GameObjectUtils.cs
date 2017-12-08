using UnityEngine;

namespace MapGeneration.Utils
{
    /// <summary>
    /// Class containing different utilities to gameobject operations.
    /// </summary>
    public static class GameObjectUtils
    {
        /// <summary>
        /// Finds all children of specified gameobject and destroys them.
        /// </summary>
        /// <param name="go">Which gameobject to find children(s) on.</param>
        /// <param name="immediate">Should it destroy them immediatly?</param>
        public static void DestroyChildren(GameObject go, bool immediate = false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                if (immediate)
                    Object.DestroyImmediate(go.transform.GetChild(i).gameObject);
                else
                    Object.Destroy(go.transform.GetChild(i).gameObject);
            }
        }
    }
}