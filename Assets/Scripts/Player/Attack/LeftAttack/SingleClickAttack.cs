using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleClickAttack : MonoBehaviour,ILeftAttacker
{
    const float DAMAGE = 10;

    PCFieldController pcFieldController => PCFieldController.instance;
    List<WeekPoint> weekPoints;
    const float SHOW_POINT_INTERVAL = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        weekPoints = pcFieldController.weekPoints;
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

                for (int i = 0; i < weekPoints.Count; i++)
                {
                    if (weekPoints[i].isSleep) 
                    {
                        weekPoints[i].ShowPoint(haveWeakPoint,SHOW_POINT_INTERVAL);
                    }
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
