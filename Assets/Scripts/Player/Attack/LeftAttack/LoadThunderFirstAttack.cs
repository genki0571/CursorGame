using System.Collections.Generic;
using UnityEngine;

public class LoadThunderFirstAttack : MonoBehaviour,ILeftAttacker
{
    const float clickDamagaSingle = 10;
    const float clickDamageDouble = 30;

    int clickCnt = 0;
    bool isWaitClick;

    const int CLICK_INTERVAL = 30;
    int clickTimer = 0;

    const float ATTACK_DAMAGE = 30;
    const float ATTACK_DAMAGE_S = 10;

    const float ATTACK_INTERVAL = 0.1f;

    const float THUNDER_RADIUS = 2f;
    const float THUNDER_RADIUS_S = 0.5f;
    const int THUNDER_MAX_NUM = 6;


    List<LoadThunder> LoadThunders = new List<LoadThunder>();

    PCFieldController pcFieldController => PCFieldController.instance;

    List<WeekPoint> weekPoints;
    const float SHOW_POINT_INTERVAL = 0.2f;

    EffectController effectController => EffectController.instance;

    // Start is called before the first frame update
    void Start()
    {
        LoadThunders = pcFieldController.loadThunders;
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
            ComboAttack(cursorTrans, hitEnemy);

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

                        effectController.PlayEffect(effectController.clickEffects, cursorTrans.position);
                    }
                }
            }
        }

        isWaitClick = true;
    }

    public void ComboAttack(Transform cursorTrans, Transform hitEnemy)
    {
        int num = 0;
        for (int i = 0; i < LoadThunders.Count; i++)
        {
            if (LoadThunders[i].isSleep)
            {
                if (num == 0)
                {
                    LoadThunders[i].Initialize(ATTACK_DAMAGE, THUNDER_RADIUS, 0, cursorTrans.position);
                    num++;
                }
                else
                {
                    LoadThunders[i].Initialize(ATTACK_DAMAGE_S, THUNDER_RADIUS_S, Random.Range(0.3f, 0.6f)
                        , cursorTrans.position + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0));
                    num++;
                }

            }

            if (num >= THUNDER_MAX_NUM)
            {
                break;
            }
        }
    }
}
