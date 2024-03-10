using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable 
{
    public void AddDamage(float damage) { }

    public void AddElement(Element element,Vector3 centerPos) { }
}
