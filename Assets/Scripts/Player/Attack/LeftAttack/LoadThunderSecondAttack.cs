using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadThunderSecondAttack : MonoBehaviour,ILeftAttacker
{
    const float ATTACK_DAMAGE = 30;
    const float ATTACK_DAMAGE_S = 10;

    const float ATTACK_INTERVAL = 0.1f;

    const float THUNDER_RADIUS = 2f;
    const float THUNDER_RADIUS_S = 0.8f;
    const int THUNDER_MAX_NUM = 6;
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
            int num = 0;
            for (int i = 0; i < LoadThunders.Count; i++)
            {
                if (LoadThunders[i].isSleep)
                {
                    if (num == 0)
                    {
                        LoadThunders[i].Initialize(ATTACK_DAMAGE, THUNDER_RADIUS,0,cursorTrans.position);
                        num++;
                    }
                    else
                    {
                        LoadThunders[i].Initialize(ATTACK_DAMAGE_S, THUNDER_RADIUS_S,Random.Range(0.3f,0.6f) 
                            ,cursorTrans.position + new Vector3(Random.Range(-1.5f,1.5f), Random.Range(-1.5f, 1.5f), 0));
                        num++;
                    }
                    
                }

                if (num >= THUNDER_MAX_NUM) 
                {
                    break;
                }
            }
            attackTimer = 0;
        }
    }
}
