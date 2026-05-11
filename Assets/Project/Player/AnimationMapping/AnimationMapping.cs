using UnityEngine;
using System.Collections.Generic;

/**
 * @brief A singleton mapping between stage names (strings) and their corresponding animation names (strings).
 */
public class AnimationMapping : Singleton<AnimationMapping>
{
    [System.Serializable]
    public struct Mapping
    {
        public string stageName;
        public string animationName;
    }

    [SerializeField]
    private List<Mapping> mappings = new List<Mapping>();

    private Dictionary<string, string> m_mappingDict;

    protected override void Awake()
    {
        base.Awake();
        
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        if (m_mappingDict != null) return;

        m_mappingDict = new Dictionary<string, string>();
        foreach (var mapping in mappings)
        {
            if (string.IsNullOrEmpty(mapping.stageName))
            {
                Debug.LogWarning("AnimationMapping: Found a mapping with an empty stage name.");
                continue;
            }

            if (!m_mappingDict.ContainsKey(mapping.stageName))
            {
                m_mappingDict.Add(mapping.stageName, mapping.animationName);
            }
            else
            {
                Debug.LogWarning($"AnimationMapping: Duplicate mapping for stage name '{mapping.stageName}'. Only the first one will be used.");
            }
        }
    }

    /**
     * @brief Retrieves the animation name associated with a given stage name.
     * @param stageName The name of the stage.
     * @return The associated animation name, or null if not found.
     */
    public string GetAnimationName(string stageName)
    {
        if (m_mappingDict == null)
        {
            InitializeDictionary();
        }

        if (m_mappingDict.TryGetValue(stageName, out var animationName))
        {
            return animationName;
        }

        Debug.LogWarning($"AnimationMapping: No animation name found for stage name '{stageName}'.");
        return null;
    }
}
