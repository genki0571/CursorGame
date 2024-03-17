using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricShock : MonoBehaviour
{
    const float DAMAGE = 10;

    const float ATTACK_INTERVAL = 0.5f;
    float attackTimer = 0;

    const int MAX_SHOCK_NUM = 3;
    int shockNum = 0;

    public bool isSleep;

    List<GameObject> shockedObjs = new List<GameObject>();

    Transform trans;
    RangeCheck rangeCheck;
    SpriteRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>();
        rangeCheck = GetComponent<RangeCheck>();
        renderer = GetComponent<SpriteRenderer>();

        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSleep) 
        {
            if (shockNum >= MAX_SHOCK_NUM) 
            {
                Reset();
            }

            if (attackTimer <= ATTACK_INTERVAL) 
            {
                attackTimer += Time.deltaTime;
            }
            if (attackTimer >= ATTACK_INTERVAL) 
            {
                GameObject nearEnemy = GetNearObj();
                if (nearEnemy != null)
                {
                    Attack(nearEnemy);
                }
                else 
                {
                    Reset();
                }
            }
        }
    }

    private GameObject GetNearObj() 
    {
        float sqrDistance = 100000;
        int num = 100000;
        for (int i = 0; i < rangeCheck.withinDamegableObjects.Count; i++)
        {
            bool already = false;
            for (int j = 0; j < shockedObjs.Count; j++)
            {
                if (rangeCheck.withinDamegableObjects[i] == shockedObjs[j]) 
                {
                    already = true;
                }
            }

            if (!already) 
            {
                float sqrDis = (trans.position - rangeCheck.withinDamegableObjects[i].transform.position).sqrMagnitude;
                if (sqrDistance >= sqrDis)
                {
                    sqrDistance = sqrDis;
                    num = i;
                }
            }
        }

        if (num != 100000) 
        {
            return rangeCheck.withinDamegableObjects[num];
        }
        else 
        {
            return null;
        }
    }

    private void Attack(GameObject target) 
    {
        IDamagable damagable = target.GetComponent<IDamagable>();
        if (damagable != null)
        {
            trans.position = target.transform.position;
            damagable.AddDamage(DAMAGE);
            damagable.AddElement(Element.Thunder,trans.position);
            shockedObjs.Add(target);
            shockNum += 1;
        }
        attackTimer = 0;

    }

    private void Reset()
    {
        isSleep = true;
        attackTimer = 0;
        shockNum = 0;
        renderer.enabled = false;
    }

    public void Initialize(GameObject target)
    {
        isSleep = false;
        Attack(target);
        renderer.enabled = true;
    }
}
