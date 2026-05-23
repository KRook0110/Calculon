using UnityEngine;

/// <summary>
/// A simple debug script that logs when the GameObject is enabled or disabled.
/// Attach this to any object you want to monitor.
/// </summary>
public class ActivationDebugger : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log($"[ActivationDebugger] {gameObject.name} has been ENABLED (Active: {gameObject.activeSelf})", gameObject);
    }

    private void OnDisable()
    {
        // Note: activeSelf might be true if the object is disabled because a parent was disabled.
        // activeInHierarchy will be false if either the object or any parent is disabled.
        Debug.Log($"[ActivationDebugger] {gameObject.name} has been DISABLED (Active Self: {gameObject.activeSelf}, Active In Hierarchy: {gameObject.activeInHierarchy})", gameObject);
    }

    private void Awake()
    {
        Debug.Log($"[ActivationDebugger] {gameObject.name} Awake. Initial State: {gameObject.activeInHierarchy}", gameObject);
    }
}
