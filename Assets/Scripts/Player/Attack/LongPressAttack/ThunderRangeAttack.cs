using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderRangeAttack : MonoBehaviour,IHoldAttacker
{
    const float THUNDER_DAMAGE = 10;

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

    public void Attack(GameObject selectEnemy, Range range)
    {
        IDamagable damagable = selectEnemy.GetComponent<IDamagable>();
        if (damagable != null)
        {
            Element element = GetElementKind(range);
            damagable.AddElement(element, holdDisplay.transform.position);

            float damage = 0;
            if (element == Element.Thunder)
            {
                damage = THUNDER_DAMAGE;
            }
            damagable.AddDamage(damage);
        }

    }

    public Element GetElementKind(Range range)
    {
        Element element = Element.Thunder;
        return element;
    }
}
