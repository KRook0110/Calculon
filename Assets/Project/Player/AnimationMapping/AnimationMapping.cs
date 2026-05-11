using UnityEngine;
using System.Collections.Generic;

/**
 * @brief A singleton mapping between PlayerProjectile types and their corresponding animation data.
 */
public class AnimationMapping : Singleton<AnimationMapping>
{
    [System.Serializable]
    public struct AnimationData
    {
        public string animationName;
        public float projectileSpawnDelay;
    }

    [System.Serializable]
    public struct Mapping
    {
        public PlayerProjectile.Type projectileType;
        public AnimationData animationData;
    }

    [SerializeField]
    private List<Mapping> mappings = new List<Mapping>();

    private Dictionary<PlayerProjectile.Type, AnimationData> m_mappingDict;

    protected override void Awake()
    {
        base.Awake();
        
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        if (m_mappingDict != null) return;

        m_mappingDict = new Dictionary<PlayerProjectile.Type, AnimationData>();
        foreach (var mapping in mappings)
        {
            if (!m_mappingDict.ContainsKey(mapping.projectileType))
            {
                m_mappingDict.Add(mapping.projectileType, mapping.animationData);
            }
            else
            {
                Debug.LogWarning($"AnimationMapping: Duplicate mapping for projectile type '{mapping.projectileType}'. Only the first one will be used.");
            }
        }
    }

    /**
     * @brief Retrieves the animation data associated with a given PlayerProjectile type.
     * @param type The type of the PlayerProjectile.
     * @return The associated AnimationData (animationName and projectileSpawnDelay), or a default one if not found.
     */
    public AnimationData GetAnimationData(PlayerProjectile.Type type)
    {
        if (m_mappingDict == null)
        {
            InitializeDictionary();
        }

        if (m_mappingDict.TryGetValue(type, out var data))
        {
            return data;
        }

        Debug.LogWarning($"AnimationMapping: No animation data found for projectile type '{type}'.");
        return default;
    }
}
