using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explotion : MonoBehaviour
{
    Transform trans;
    

    RangeCheck[] rangeChecks;
    RangeCheck rangeCheckS;
    RangeCheck rangeCheckM;
    RangeCheck rangeCheckL;

    SpriteRenderer rendererS;
    SpriteRenderer rendererM;
    SpriteRenderer rendererL;

    const float INTERVAL_S = 0.2f;
    const float INTERVAL_M = 0.4f;
    const float INTERVAL_L = 0.6f;
    const float EXPLOTION_INTERVAL = 0.8f;
    float timer = 0;

    bool attackedS;
    bool attackedM;
    bool attackedL;

    const float DAMAGE_S = 50;
    const float DAMAGE_M = 30;
    const float DAMAGE_L = 10;

    public bool isSleep;

    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>();

        rangeChecks = GetComponentsInChildren<RangeCheck>();
        rangeCheckS = rangeChecks[0];
        rangeCheckM = rangeChecks[1];
        rangeCheckL = rangeChecks[2];

        rendererS = rangeCheckS.GetComponent<SpriteRenderer>();
        rendererM = rangeCheckM.GetComponent<SpriteRenderer>();
        rendererL = rangeCheckL.GetComponent<SpriteRenderer>();

        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSleep)
        {
            if (timer <= EXPLOTION_INTERVAL)
            {
                timer += Time.deltaTime;
            }
            if (timer >= EXPLOTION_INTERVAL) 
            {
                Reset();
            }

            if (timer >= INTERVAL_S) 
            {
                if (!attackedS) 
                {
                    rendererS.enabled = true;
                    Attack(rangeCheckS.withinDamegableObjects, DAMAGE_S);
                    attackedS = true;
                }
            }
            if (timer >= INTERVAL_M)
            {
                if (!attackedM)
                {
                    rendererS.enabled = false;
                    rendererM.enabled = true;
                    Attack(rangeCheckS.withinDamegableObjects, DAMAGE_M);
                    attackedM = true;
                }
            }
            if (timer >= INTERVAL_L)
            {
                if (!attackedL)
                {
                    rendererM.enabled = false;
                    rendererL.enabled = true;
                    Attack(rangeCheckL.withinDamegableObjects, DAMAGE_L);
                    attackedL = true;
                }
            }
        }
    }

    private void Attack(List<GameObject> objs,float damage) 
    {
        for (int i = 0; i < objs.Count; i++)
        {
            IDamagable damagable = objs[i].GetComponent<IDamagable>();
            if (damagable != null) 
            {
                damagable.AddDamage(damage);
            }
        }
    }

    private void Reset()
    {
        rendererS.enabled = false;
        rendererM.enabled = false;
        rendererL.enabled = false;

        attackedS = false;attackedM = false;attackedL = false;

        timer = 0;
        trans.transform.position = new Vector3(0,70,0);
        isSleep = true;
    }

    public void Initialize(Vector3 pos)
    {
        trans.transform.position = pos;
        isSleep = false;
    }
}
