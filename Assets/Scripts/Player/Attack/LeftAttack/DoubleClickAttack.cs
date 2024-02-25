﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleClickAttack : MonoBehaviour,ILeftAttacker
{
    const float CLICK_DAMAGE_SINGLE = 10;
    const float CLICK_DAMAGE_DOUBLE = 30;

    int clickCnt = 0;
    bool isWaitClick;

    const int CLICK_INTERVAL = 30;
    int clickTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isWaitClick)
        {
            clickTimer += 1;
            if (clickTimer >= CLICK_INTERVAL) 
            {
                isWaitClick = false;
            }
        }
        else 
        {
            clickTimer = 0;
        }
    }

    public void Attack(Transform hitEnemy)
    {
        if (hitEnemy)
        {
            IDamagable damageTarget = hitEnemy.GetComponent<IDamagable>();
            if (damageTarget != null)
            {
                float damage = 0;
                if (clickCnt == 0)
                {
                    damage = CLICK_DAMAGE_SINGLE;
                    clickCnt = 1;
                }
                else if (clickCnt == 1) 
                {
                    damage = CLICK_DAMAGE_DOUBLE;
                    clickCnt = 0;
                }

                damageTarget.AddDamage(damage);
            }
        }
    }
}
