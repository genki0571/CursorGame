using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleClickAttack : MonoBehaviour,ILeftAttacker
{
    const float clickDamagaSingle = 10;
    const float clickDamageDouble = 30;
    const float clickDamageTriple = 60;

    int clickCnt = 0;
    bool isWaitClick;

    const int CLICK_INTERVAL = 30;
    int clickTimer = 0;

    PCFieldController pcFieldController => PCFieldController.instance;
    List<WeekPoint> weekPoints;
    const float SHOW_POINT_INTERVAL = 0.2f;

    EffectController effectController => EffectController.instance;


    // Start is called before the first frame update
    void Start()
    {
        weekPoints = pcFieldController.weekPoints;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isWaitClick)
        {
            clickTimer += 1;
            if (clickTimer >= CLICK_INTERVAL)
            {
                isWaitClick = false;
                clickCnt = 0;
            }
        }
        else
        {
            clickTimer = 0;
        }
    }

    public void Attack(Transform cursorTrans, Transform hitEnemy)
    {
        if (clickCnt == 2) //連三回目の攻撃
        {
            if (hitEnemy)
            {
                float damage = clickDamageTriple;

                bool damaged = false;

                IHaveWeakPoint haveWeakPoint = hitEnemy.GetComponent<IHaveWeakPoint>();
                if (haveWeakPoint != null)
                {
                    if (haveWeakPoint.IsAttackWeekPoint(this.transform.position))
                    {
                        haveWeakPoint.AddWeakDamage(damage);
                        damaged = true;
                        effectController.PlayEffect(effectController.clickCriticalEffects, cursorTrans.position);
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

                        effectController.PlayEffect(effectController.clickEffects, cursorTrans.position);
                    }
                }
            }

            clickCnt = 0;
            isWaitClick = false;
        }
        else
        {
            if (hitEnemy)
            {
                float damage = 0;
                if (clickCnt == 0)
                {
                    damage = clickDamagaSingle;
                    clickCnt = 1;
                }
                else if (clickCnt == 1)
                {
                    damage = clickDamageDouble;
                    clickCnt = 2;
                }

                bool damaged = false;

                IHaveWeakPoint haveWeakPoint = hitEnemy.GetComponent<IHaveWeakPoint>();
                if (haveWeakPoint != null)
                {
                    if (haveWeakPoint.IsAttackWeekPoint(this.transform.position))
                    {
                        haveWeakPoint.AddWeakDamage(damage);
                        damaged = true;
                        effectController.PlayEffect(effectController.clickCriticalEffects, cursorTrans.position);
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

                        effectController.PlayEffect(effectController.clickEffects,cursorTrans.position);
                    }
                }
            }

        }


        isWaitClick = true;
    }
}
