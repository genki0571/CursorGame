using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadThunderFirstAttack : MonoBehaviour,ILeftAttacker
{
    const float ATTACK_DAMAGE = 30;

    const float ATTACK_INTERVAL = 0.1f;
    float attackTimer = 0;

    bool isReady;
    List<LoadThunder> LoadThunders = new List<LoadThunder>();

    PCFieldController pcFieldController => PCFieldController.instance;

    // Start is called before the first frame update
    void Start()
    {
        LoadThunders = pcFieldController.loadThunders;
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

    public void Attack(Transform cursorTrans, Transform hitEnemy)
    {
        if (isReady)
        {
            for (int i = 0; i < LoadThunders.Count; i++)
            {
                if (LoadThunders[i].isSleep)
                {
                    LoadThunders[i].Initialize(ATTACK_DAMAGE, cursorTrans.position);
                    break;
                }
            }
            attackTimer = 0;
        }
    }
}
