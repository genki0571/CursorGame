using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWall : MonoBehaviour
{
    const float DAMAGE = 10;

    SpriteRenderer renderer;

    public bool isSleep;

    const float ATTACK_INTERVAL = 0.5f;
    float attackTimer = 0;

    RangeCheck rangeCheck;
    List<GameObject> withinEnemies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        

        renderer = GetComponent<SpriteRenderer>();
        rangeCheck = GetComponent<RangeCheck>();
        withinEnemies = rangeCheck.withinDamegableObjects;
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSleep)
        {
            if (attackTimer <= ATTACK_INTERVAL)
            {
                attackTimer += Time.deltaTime;
            }

            if (attackTimer >= ATTACK_INTERVAL)
            {
                attackTimer = 0;
                Attack();
            }
        }
        else 
        {
            attackTimer = 0;
        }
    }


    private void Attack() 
    {
        for (int i = 0; i < withinEnemies.Count; i++)
        {
            IDamagable damagable = withinEnemies[i].transform.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.AddDamage(DAMAGE);
            }
        }
    }

    void Reset()
    {
        isSleep = true;
        renderer.enabled = false;
    }

    public void Initialize()
    {
        isSleep = false;
        renderer.enabled = true;
    }
}
