using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHaveWeakPoint
{
    public bool IsAttackWeekPoint(Vector3 pos) 
    {
        return true;
    }

    public Vector3 GetWeekPoint() 
    {
        return Vector3.zero;
    }

    public void AddWeakDamage(float damage) { }
}
