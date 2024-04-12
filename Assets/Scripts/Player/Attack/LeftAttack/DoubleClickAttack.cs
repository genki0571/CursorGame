using System.Collections;
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


    PCFieldController pcFieldController => PCFieldController.instance;
    List<WeekPoint> weekPoints;
    const float SHOW_POINT_INTERVAL = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        weekPoints = pcFieldController.weekPoints;
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

    public void Attack(Transform cursorTrans,Transform hitEnemy)
    {
        if (hitEnemy)
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

            bool damaged = false;

            IHaveWeakPoint haveWeakPoint = hitEnemy.GetComponent<IHaveWeakPoint>();
            if (haveWeakPoint != null)
            {
                if (haveWeakPoint.IsAttackWeekPoint(this.transform.position))
                {
                    haveWeakPoint.AddWeakDamage(damage);
                    damaged = true;
                }

                for (int i = 0; i < weekPoints.Count; i++)
                {
                    if (weekPoints[i].isSleep)
                    {
                        weekPoints[i].ShowPoint(haveWeakPoint, SHOW_POINT_INTERVAL);
                    }
                }
            }

            if (!damaged)
            {
                IDamagable damageTarget = hitEnemy.GetComponent<IDamagable>();
                if (damageTarget != null)
                {
                    damageTarget.AddDamage(damage);
                }
            }
        }
    }
}
