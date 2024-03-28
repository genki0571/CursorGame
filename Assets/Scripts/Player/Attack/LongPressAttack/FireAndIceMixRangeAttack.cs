using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAndIceMixRangeAttack : MonoBehaviour,IHoldAttacker
{
    PCFieldController pcFieldController => PCFieldController.instance;
    List<Explotion> explotions = new List<Explotion>(); 

    const float FIRE_DAMAGE = 10;
    const float ICE_DAMAGE = 5;

    GameObject holdDisplay;

    // Start is called before the first frame update
    void Start()
    {
        holdDisplay = GetComponent<PlayerAttack>().holdDisplay;

        GameObject explotionPool = new GameObject("ExplotionPool");
        for (int i = 0; i < 5; i++)
        {
            GameObject exObj = Instantiate(pcFieldController.explotion,transform.position,Quaternion.identity);
            Explotion ex = exObj.GetComponent<Explotion>();
            explotions.Add(ex);
            exObj.transform.parent = explotionPool.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Attack(GameObject selectEnemy, Range range)
    {
        Element element = GetElementKind(range);
        float damage = 0;
        if (element == Element.Fire)
        {
            IDamagable damagable = selectEnemy.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.AddElement(element, holdDisplay.transform.position);
            }
            damage = FIRE_DAMAGE;
            damagable.AddDamage(damage);
        }
        else if (element == Element.Ice)
        {
            IDamagable damagable = selectEnemy.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.AddElement(element, holdDisplay.transform.position);
            }
            damage = ICE_DAMAGE;
            damagable.AddDamage(damage);
        }
        else if (element == Element.FireAndIce)
        {
            for (int i = 0; i < explotions.Count; i++)
            {
                if (explotions[i].isSleep)
                {
                    explotions[i].Initialize(holdDisplay.transform.position);
                    break;
                }
            }
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
