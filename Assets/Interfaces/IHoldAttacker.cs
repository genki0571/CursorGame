﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHoldAttacker
{
    void Attack(List<GameObject> selectEnemy,Range range) { }

    Element GetElementKind(Range range) 
    {
        Element element = Element.Normal;
        return element;
    }

    float GetMaxLength() 
    {
        return 0;
    }
}
