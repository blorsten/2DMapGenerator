using UnityEngine;

namespace MapGeneration.Utils
{
    /// <summary>
    /// Implementation of Singleton design pattern that supports Unity's <see cref="MonoBehaviour"/>
    /// </summary>
    /// <typeparam name="T">The type which this singleton is of.</typeparam>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        //If true then the application is closing.
        private static bool _isQuitting; 

        //Should the single object persist between scene loading.
        public bool DontDestroyOnLoadConfig = true;

        //Reference to the instanced singleton object.
        private static T instance;

        /// <summary>
        /// Returns the instance of this singleton.
        /// </summary>
        public static T Instance
        {
            get
            {
                //If the application is closing dont try to create a new singleton object.
                if (_isQuitting)
                    return null;

                //If we havent created a new instance, try to do so.
                if (instance == null)
                {
                    //First try and find an already instantiated instnace.
                    instance = (T)FindObjectOfType(typeof(T));
                    
                    //If that dident work create a new one.
                    if (instance == null)
                    {
                        GameObject newInstance = new GameObject(typeof(T).ToString());
                        instance = newInstance.AddComponent<T>();

                        if (Application.isPlaying)
                            DontDestroyOnLoad(newInstance);

                        return instance;
                    }
                }

                return instance;
            }
        }
        
        /// <summary>
        /// Check if the instance has been instantiated.
        /// </summary>
        /// <returns>True if instance is not null</returns>
        public static bool CheckSanity()
        {
            if (instance != null)
                return true;

            return false;
        }

        protected virtual void Awake()
        {
            if (DontDestroyOnLoadConfig && Application.isPlaying)
                DontDestroyOnLoad(gameObject);
        }

        private void OnApplicationQuit()
        {
            if (!Application.isPlaying)
                _isQuitting = true;
        }
    }
}