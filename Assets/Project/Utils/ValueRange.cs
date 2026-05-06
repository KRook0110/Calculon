using System;
using System.Numerics;
using UnityEngine;

[System.Serializable]
public struct ValueRange
{
    public float min;
    public float max;

    public ValueRange(float min, float max)
    {
        // Now you can use comparison operators!
        if (min > max)
        {
            throw new ArgumentException("Min cannot be greater than Max");
        }
        this.min = min;
        this.max = max;
    }

    public float Random()
    {
        return UnityEngine.Random.Range(min, max);
    }
}