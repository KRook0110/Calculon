using System;
using UnityEngine;

public interface IProjectile
{
    public Transform Target{get;set;}
    public event Action<GameObject> OnHit;
}