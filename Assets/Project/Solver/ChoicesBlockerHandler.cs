using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class ChoicesBlockerHandler : Singleton<ChoicesBlockerHandler>
{
    [Header("dependencies")]
    [SerializeField]
    private GameObject blockerUI;

    private HashSet<GameObject> _objectsBlocking = new HashSet<GameObject>();

    public bool blocked { get; private set; } = false;


    // @params : object blocking only as an ID, the object itself doesn't change
    public void Block(GameObject objectBlocking)
    {
        Debug.Log($"Attempt Block {objectBlocking.name}");
        bool present = _objectsBlocking.Add(objectBlocking);
        if (!present)
        {
            Debug.Log("GameObject not found");
        }
        RefreshBlock();
    }

    public bool IsBlocking(GameObject go)
    {
        return _objectsBlocking.Contains(go);
    }

    public void UnBlock(GameObject objectUnblocking)
    {
        Debug.Log($"Attempt UnBlock {objectUnblocking.name}");
        bool removed = _objectsBlocking.Remove(objectUnblocking);
        if (!removed)
        {
            Debug.LogWarning($"Failed to Unblock, GameObject of name {objectUnblocking.name} not found");
        }
        RefreshBlock();
    }

    void RefreshBlock()
    {
        blocked = _objectsBlocking.Count > 0;
        blockerUI.SetActive(blocked);
    }

    void Start()
    {
        RefreshBlock();
    }

    void HandleCooldownBlocking()
    {
        Block(MutlipleChoiceCooldown.Instance.gameObject);
    }
    void HandleCooldownUnBlocking()
    {
        UnBlock(MutlipleChoiceCooldown.Instance.gameObject);
    }
    void OnEnable()
    {
        MutlipleChoiceCooldown.Instance.OnUsable += HandleCooldownUnBlocking;
        MutlipleChoiceCooldown.Instance.OnCooldown += HandleCooldownBlocking;
    }

    void OnDisable()
    {
        MutlipleChoiceCooldown.Instance.OnUsable -= HandleCooldownUnBlocking;
        MutlipleChoiceCooldown.Instance.OnCooldown -= HandleCooldownBlocking;
    }
}
