﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILongPressAttacker
{
    void Attack(GameObject selectEnemy,Range range) { }

    Element GetElementKind(Range range) 
    {
        Element element = Element.Normal;
        return element;
    }
}
