using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAttack : MonoBehaviour,ILeftAttacker
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(Transform hitEnemy)
    {
        if (hitEnemy) 
        {
            IDamagable damageTarget = hitEnemy.GetComponent<IDamagable>();
            if (damageTarget != null)
            {
                damageTarget.AddDamage(10);
            }
        }
    }
}
