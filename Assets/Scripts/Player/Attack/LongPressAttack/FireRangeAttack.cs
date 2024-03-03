using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRangeAttack : MonoBehaviour,ILongPressAttacker
{
    Element element = Element.Fire;
    const float FIRE_DAMAGE = 10; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(GameObject selectEnemy,Range range)
    {
        IDamagable damagable = selectEnemy.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.AddDamage(FIRE_DAMAGE);
            damagable.AddElement(GetElementKind(range));
        }
    }

    public Element GetElementKind(Range range)
    {
        if (range == Range.Mix)
        {
            return Element.Empty;
        }
        else
        {
            return element;
        }
    }
}
