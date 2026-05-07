using System;
using System.Collections;
using UnityEngine;

public abstract class PlayerProjectile : MonoBehaviour
{
    public Enemy target;
    public Action<PlayerProjectile> OnHit;
}