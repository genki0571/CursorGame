using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerAttack : MonoBehaviour,ILeftAttacker
{

    bool isReady;
    List<Hammer> hammers = new List<Hammer>();

    public float clickDamageSingle = 10;
    public float clickDamageDouble = 30;
    public float attackHammerDamage = 35;

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
        hammers = pcFieldController.hammers;
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
                clickCnt = 0;
            }
        }
        else
        {
            clickTimer = 0;
        }
    }

    public void Attack(Transform cursorTrans,Transform hitEnemy)
    {

        if (clickCnt == 2) //連三回目の攻撃
        {
            for (int i = 0; i < hammers.Count; i++)
            {
                if (hammers[i].isSleep)
                {
                    hammers[i].Initialize(attackHammerDamage, cursorTrans.position);
                    break;
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
                    damage = clickDamageSingle;
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

                        effectController.PlayEffect(effectController.clickEffects, cursorTrans.position);
                    }
                }
            }
        }

        clickTimer = 0;
        isWaitClick = true;
    }
}
