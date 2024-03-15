using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalRangeAttack : MonoBehaviour,IHoldAttacker
{
    Element element = Element.Normal;
    const float DAMAGE = 5;

    GameObject holdDisplay;

    // Start is called before the first frame update
    void Start()
    {
        holdDisplay = GetComponent<PlayerAttack>().holdDisplay;
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
            damagable.AddDamage(DAMAGE);
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
