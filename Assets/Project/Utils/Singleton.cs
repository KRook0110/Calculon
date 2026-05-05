
using TMPro;
using UnityEngine;

/**
 * @brief A generic implementation of the Singleton pattern for MonoBehaviour scripts.
 * * This class ensures that only one instance of a specific component exists in the scene.
 * It provides global access via the Instance property and handles automatic instantiation 
 * if no instance is found in the scene.
 * * @tparam T The type of the class inheriting from the Singleton.
 */
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_instance;

    /**
     * @brief Accesses the global instance of the Singleton.
     * @return The existing instance of type T, or a newly created one if none exists.
     */
    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindAnyObjectByType<T>();

                if (m_instance == null)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    m_instance = go.AddComponent<T>();
                }
            }

            return m_instance;
        }
    }

    /**
     * @brief Internal initialization logic to assign the instance and handle persistence.
     * * @warning **IMPORTANT FOR CLIENTS:** If you override the Awake method in a derived class, 
     * you **MUST** call `base.Awake()` at the start of your method. Failure to do so will 
     * prevent the Singleton from initializing correctly and may result in null references 
     * or duplicate instances.
     * * @note By default, this implementation uses DontDestroyOnLoad to persist across scenes.
     */
    protected virtual void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this as T;
            // DontDestroyOnLoad(gameObject);
        }
        else if (m_instance != this)
        {
            // Destroy duplicate instances
            Destroy(gameObject);
        }
    }

    /**
    * @brief Cleans up the static instance reference when the GameObject is destroyed.
    * * This is critical for preventing memory leaks in the Unity Editor. It ensures 
    * that the static reference is nullified so that Unity can fully garbage collect 
    * the object when a scene is closed or Play Mode is stopped.
    * * @note If you override this in a derived class, you MUST call `base.OnDestroy()`.
    */
    protected virtual void OnDestroy()
    {
        if (m_instance == null)
        {
            m_instance = null;
        }
    }
}