using System.Collections;
using UnityEngine;

public class AOEExplosionEffectHandler : MonoBehaviour
{
    [SerializeField]
    private float _destroyTime = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DestroyRoutine());
    }

    IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(_destroyTime);
        Destroy(this);
    }

}
