using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAndIceMixRangeAttack : MonoBehaviour,ILongPressAttacker
{
    const float FIRE_DAMAGE = 10;
    const float ICE_DAMAGE = 5;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Attack(GameObject selectEnemy, Range range)
    {
        IDamagable damagable = selectEnemy.GetComponent<IDamagable>();
        if (damagable != null)
        {
            Element element = GetElementKind(range);
            damagable.AddElement(element);

            float damage = 0;
            if (element == Element.Fire)
            {
                damage = FIRE_DAMAGE;
            }
            else if (element == Element.Ice)
            {
                damage = ICE_DAMAGE;
            }
            else if (element == Element.FireAndIce)
            {

            }
            damagable.AddDamage(damage);
        }
        
    }

    public Element GetElementKind(Range range)
    {
        Element element = Element.Normal;
        if (range == Range.Right)
        {
            element = Element.Fire;
        }
        else if (range == Range.Left)
        {
            element = Element.Ice;
        }
        else if(range == Range.Mix)
        {
            element = Element.FireAndIce;
        }
        return element;
    }
}
