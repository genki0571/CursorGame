using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadThunder : MonoBehaviour
{
    float attackDamage;

    const float ATTACK_INTERVAL = 1f;
    float attackInterval = 0;
    float attackTimer = 0;

    public bool isSleep;

    List<GameObject> withinDamagables;

    RangeCheck rangeCheck;
    SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        rangeCheck = GetComponentInChildren<RangeCheck>();
        withinDamagables = rangeCheck.withinDamegableObjects;
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSleep)
        {
            if (attackTimer <= attackInterval)
            {
                attackTimer += Time.deltaTime;
            }

            if (attackTimer >= attackInterval)
            {
                Attack();
                Reset();
            }
        }

    }

    private void Attack()
    {
        for (int i = 0; i < withinDamagables.Count; i++)
        {
            bool damaged = false;

            IHaveWeakPoint haveWeakPoint = withinDamagables[i].GetComponent<IHaveWeakPoint>();
            if (haveWeakPoint != null)
            {
                if (haveWeakPoint.IsAttackWeekPoint(this.transform.position))
                {
                    haveWeakPoint.AddWeakDamage(attackDamage);
                    damaged = true;
                }
            }

            if (!damaged)
            {
                IDamagable damagable = withinDamagables[i].GetComponent<IDamagable>();
                if (damagable != null)
                {
                    damagable.AddDamage(attackDamage);
                }
            }
        }
    }

    void Reset()
    {
        renderer.enabled = false;
        isSleep = true;
        attackInterval = ATTACK_INTERVAL;
    }

    /// <summary>
    /// Thunderを発生する
    /// </summary>
    /// <param name="damage">ダメージ</param>
    /// <param name="radius">判定の半径</param>
    /// <param name="delay">発生の遅れ時間</param>
    /// <param name="pos">発生場所</param>
    public void Initialize(float damage, float radius ,float delay ,Vector3 pos)
    {
        this.transform.position = pos;
        renderer.enabled = true;
        transform.localScale = new Vector3(radius,radius,radius);
        attackInterval = ATTACK_INTERVAL + delay;
        attackDamage = damage;
        isSleep = false;
        attackTimer = 0;
    }
}
