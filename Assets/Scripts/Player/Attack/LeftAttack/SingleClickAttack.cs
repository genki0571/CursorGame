using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleClickAttack : MonoBehaviour,ILeftAttacker
{
    const float DAMAGE = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(Transform cursorTrans, Transform hitEnemy)
    {
        if (hitEnemy) 
        {
            bool damaged = false;

            IHaveWeakPoint haveWeakPoint = hitEnemy.GetComponent<IHaveWeakPoint>();
            if (haveWeakPoint != null)
            {
                if (haveWeakPoint.IsAttackWeekPoint(this.transform.position))
                {
                    haveWeakPoint.AddWeakDamage(DAMAGE);
                    damaged = true;
                }
            }

            if (!damaged) 
            {
                IDamagable damageTarget = hitEnemy.GetComponent<IDamagable>();
                if (damageTarget != null)
                {
                    damageTarget.AddDamage(DAMAGE);
                }
            }
        }
    }
}
