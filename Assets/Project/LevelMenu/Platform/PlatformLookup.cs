using System.Collections.Generic;
using UnityEngine;

public class PlatformLookup : Singleton<PlatformLookup>
{
    private Dictionary<LevelData, LevelPlatform> _platformLookup = new Dictionary<LevelData, LevelPlatform>();
    private Dictionary<LevelData, List<LevelData>> _previousLevels = new Dictionary<LevelData, List<LevelData>>();

    public void RegisterPlatform(LevelData data, LevelPlatform platform)
    {
        if (data == null) return;
        
        if (_platformLookup.ContainsKey(data))
        {
            Debug.LogError($"Duplicate LevelData assignment detected for level: {data.levelName}. Multiple platforms are trying to register with the same LevelData.");
            return;
        }
        
        _platformLookup[data] = platform;

        // Automatically populate backward connections based on nextLevels
        if (data.nextLevels != null)
        {
            foreach (LevelData next in data.nextLevels)
            {
                if (next == null) continue;

                if (!_previousLevels.ContainsKey(next))
                {
                    _previousLevels[next] = new List<LevelData>();
                }

                if (!_previousLevels[next].Contains(data))
                {
                    _previousLevels[next].Add(data);
                }
            }
        }
    }

    public LevelPlatform GetPlatform(LevelData data)
    {
        if (data == null) return null;
        _platformLookup.TryGetValue(data, out LevelPlatform platform);
        return platform;
    }

    public List<LevelPlatform> GetPath(LevelPlatform start, LevelPlatform end)
    {
        if (start == null || end == null) return new List<LevelPlatform>();

        Queue<LevelPlatform> queue = new Queue<LevelPlatform>();
        Dictionary<LevelPlatform, LevelPlatform> parents = new Dictionary<LevelPlatform, LevelPlatform>();

        queue.Enqueue(start);
        parents[start] = null;

        while (queue.Count > 0)
        {
            LevelPlatform current = queue.Dequeue();

            if (current == end)
            {
                List<LevelPlatform> path = new List<LevelPlatform>();
                LevelPlatform node = end;
                while (node != null)
                {
                    path.Add(node);
                    node = parents[node];
                }
                path.Reverse();
                return path;
            }

            if (current.Level != null)
            {
                // Check forward connections (nextLevels)
                if (current.Level.nextLevels != null)
                {
                    foreach (LevelData nextLevel in current.Level.nextLevels)
                    {
                        if (nextLevel != null)
                        {
                            LevelPlatform nextPlatform = GetPlatform(nextLevel);
                            if (nextPlatform != null && !parents.ContainsKey(nextPlatform))
                            {
                                parents[nextPlatform] = current;
                                queue.Enqueue(nextPlatform);
                            }
                        }
                    }
                }

                // Check backward connections (automatically generated)
                if (_previousLevels.TryGetValue(current.Level, out List<LevelData> prevLevels))
                {
                    foreach (LevelData prevLevel in prevLevels)
                    {
                        if (prevLevel != null)
                        {
                            LevelPlatform prevPlatform = GetPlatform(prevLevel);
                            if (prevPlatform != null && !parents.ContainsKey(prevPlatform))
                            {
                                parents[prevPlatform] = current;
                                queue.Enqueue(prevPlatform);
                            }
                        }
                    }
                }
            }
        }

        return new List<LevelPlatform>();
    }

    /// <summary>
    /// Gets the horizontal boundaries (minimum and maximum X coordinates) of all registered and unlocked platforms.
    /// Returns true if at least one platform is registered; otherwise, false.
    /// </summary>
    public bool TryGetPlatformBoundaries(out float minX, out float maxX)
    {
        minX = float.MaxValue;
        maxX = float.MinValue;

        foreach (var platform in _platformLookup.Values)
        {
            if (platform != null && platform.Level != null)
            {
                // Filter by unlocked state if LevelState is present
                if (LevelState.HasInstance && !LevelState.Instance.IsUnlocked(platform.Level.levelName))
                {
                    continue;
                }

                float x = platform.transform.position.x;
                if (x < minX) minX = x;
                if (x > maxX) maxX = x;
            }
        }

        if (minX == float.MaxValue || maxX == float.MinValue)
        {
            // Fallback: If no unlocked platforms are registered yet, use all platforms
            foreach (var platform in _platformLookup.Values)
            {
                if (platform != null)
                {
                    float x = platform.transform.position.x;
                    if (x < minX) minX = x;
                    if (x > maxX) maxX = x;
                }
            }
        }

        if (minX == float.MaxValue || maxX == float.MinValue)
        {
            minX = 0f;
            maxX = 0f;
            return false;
        }

        return true;
    }
}
