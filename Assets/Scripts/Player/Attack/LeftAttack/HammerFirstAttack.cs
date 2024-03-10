using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerFirstAttack : MonoBehaviour,ILeftAttacker
{
    const float ATTACK_DAMAGE = 20;

    const float ATTACK_INTERVAL = 0.1f;
    float attackTimer = 0;

    bool isReady;
    List<Hammer> hammers = new List<Hammer>();

    PCFieldController pcFieldController => PCFieldController.instance;

    // Start is called before the first frame update
    void Start()
    {
        hammers = pcFieldController.hammers;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTimer <= ATTACK_INTERVAL) 
        {
            attackTimer += Time.deltaTime;
        }

        if (attackTimer >= ATTACK_INTERVAL)
        {
            isReady = true;
        }
        else 
        {
            isReady = false;
        }
    }

    public void Attack(Transform cursorTrans,Transform hitEnemy)
    {
        if (isReady) 
        {
            for (int i = 0; i < hammers.Count; i++)
            {
                if (hammers[i].isSleep)
                {
                    hammers[i].Initialize(ATTACK_DAMAGE, cursorTrans.position);
                    break;
                }
            }
            attackTimer = 0;
        }
    }
}
