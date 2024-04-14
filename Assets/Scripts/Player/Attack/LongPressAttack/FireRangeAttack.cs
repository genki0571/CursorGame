using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRangeAttack : MonoBehaviour,IHoldAttacker
{
    Element element = Element.Fire;
    const float FIRE_DAMAGE = 10;

    const float MAX_LENGTH = 5;

    GameObject holdDisplay;
    PCFieldController pcFieldController => PCFieldController.instance;
    GameObject fireAreaObj;
    List<FireArea> fireAreas = new List<FireArea>();

    // Start is called before the first frame update
    void Start()
    {
        holdDisplay = GetComponent<PlayerAttack>().holdDisplay;

        fireAreaObj = pcFieldController.fireArea;
        GameObject fireAreaPool = new GameObject("FireAreaPool");
        for (int i = 0; i < 3; i++)
        {
            GameObject area = Instantiate(fireAreaObj, new Vector3(0, 95, 0), Quaternion.identity);
            FireArea fireArea = area.GetComponent<FireArea>();
            fireAreas.Add(fireArea);
            area.transform.parent = fireAreaPool.transform;
        }
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
            damagable.AddElement(GetElementKind(range), holdDisplay.transform.position);
        }

        for (int i = 0; i < fireAreas.Count; i++)
        {
            if (fireAreas[i].isSleep) 
            {
                fireAreas[i].Initialize(holdDisplay.transform);
                break;
            }
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

    public float GetMaxLength()
    {
        return MAX_LENGTH;
    }
}
