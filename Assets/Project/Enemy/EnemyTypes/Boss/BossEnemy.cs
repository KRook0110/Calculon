using System;
using System.Collections;
using UnityEngine;

public class BossEnemy : BasicEnemy
{

    [Serializable]
    public class SummoningInfo
    {
        public float delay;
        public Enemy summoningPrefab;
        public int amount;
    }


    [Header("Summoning Info")]
    [SerializeField]
    private SummoningInfo[] _summoningInfos;

    [Header("Summoning Animation Settings")]
    [SerializeField]
    private string _summoningAnimationName;
    [SerializeField]
    private float _summoningAnimationDuration;
    [SerializeField]
    private float _summoningDelay;

    private int _summoningAnimationNameHash;
    private bool _isSummoning;

    protected override void Awake()
    {
        base.Awake();
        _summoningAnimationNameHash = Animator.StringToHash(_summoningAnimationName);
    }
    protected override void Start()
    {
        base.Start();
        foreach (var info in _summoningInfos)
        {
            StartCoroutine(Summoning(info));
        }
    }
    override protected bool IsAllowedToMove()
    {
        return base.IsAllowedToMove() && !_isSummoning;
    }

    IEnumerator Summoning(SummoningInfo info)
    {
        yield return new WaitForSeconds(info.delay);
        Debug.Log($"Summoning: Start Summoning {info.summoningPrefab.name}");
        _isSummoning = true;
        _animator.Play(_summoningAnimationNameHash);
        yield return new WaitForSeconds(_summoningDelay);

        for (int i = 0; i < info.amount; i++)
        {
            FightCoordinator.Instance.SpawnEnemy(info.summoningPrefab.gameObject, transform.position, transform.rotation);
        }
        yield return new WaitForSeconds(_summoningAnimationDuration);
        Debug.Log($"Summoning: End Summoning {info.summoningPrefab.name}");
        _isSummoning = false;
    }
}
