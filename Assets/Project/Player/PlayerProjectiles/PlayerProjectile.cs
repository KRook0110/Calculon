using System;
using System.Collections;
using UnityEngine;

public abstract class PlayerProjectile : MonoBehaviour
{
    public enum Type
    {
        Basic, Multi, AOE
    }
    public Type type;
    public Enemy target;
    public Action<PlayerProjectile> OnHit;
}