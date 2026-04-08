using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField]
    private PlayerEntity _playerEntity;
    [SerializeField]
    private Transform _targetUI;

    void RefreshHealthBar(PlayerEntity.PlayerDamageInfo info)
    {
        float ratio = 0f;
        if (info.maxHealth > 0) {
            ratio = Mathf.Clamp01((float)info.remainingHealth / info.maxHealth);
        }
        _targetUI.localScale = new Vector3(
            ratio,
            _targetUI.localScale.y,
            _targetUI.localScale.z
        );
    }

    void OnEnable()
    {
        _playerEntity.OnDamage += RefreshHealthBar;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
