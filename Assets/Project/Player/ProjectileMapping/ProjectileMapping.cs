using UnityEngine;
using System.Collections.Generic;

/**
 * @brief A singleton mapping between QuestionStage names and their corresponding PlayerProjectile prefabs.
 */
public class ProjectileMapping : Singleton<ProjectileMapping>
{
    [System.Serializable]
    public struct Mapping
    {
        public string stageName;
        public PlayerProjectile projectilePrefab;
    }

    [SerializeField]
    private List<Mapping> mappings = new List<Mapping>();

    private Dictionary<string, PlayerProjectile> m_mappingDict;

    protected override void Awake()
    {
        base.Awake();
        // DontDestroyOnLoad(this);
        
        // Ensure the dictionary is initialized even if Awake is called multiple times (though Singleton handles it)
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        if (m_mappingDict != null) return;

        m_mappingDict = new Dictionary<string, PlayerProjectile>();
        foreach (var mapping in mappings)
        {
            if (string.IsNullOrEmpty(mapping.stageName))
            {
                Debug.LogWarning("ProjectileMapping: Found a mapping with an empty stage name.");
                continue;
            }

            if (!m_mappingDict.ContainsKey(mapping.stageName))
            {
                m_mappingDict.Add(mapping.stageName, mapping.projectilePrefab);
            }
            else
            {
                Debug.LogWarning($"ProjectileMapping: Duplicate mapping for stage name '{mapping.stageName}'. Only the first one will be used.");
            }
        }
    }

    /**
     * @brief Retrieves the PlayerProjectile prefab associated with a given QuestionStage name.
     * @param stageName The name of the QuestionStage.
     * @return The associated PlayerProjectile prefab, or null if not found.
     */
    public PlayerProjectile GetProjectilePrefab(string stageName)
    {
        if (m_mappingDict == null)
        {
            InitializeDictionary();
        }

        if (m_mappingDict.TryGetValue(stageName, out var prefab))
        {
            return prefab;
        }

        Debug.LogWarning($"ProjectileMapping: No projectile prefab found for stage name '{stageName}'.");
        return null;
    }
}
