using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderRangeAttack : MonoBehaviour,IHoldAttacker
{
    const float THUNDER_DAMAGE = 10;

    const float MAX_LENGTH = 5;

    GameObject holdDisplay;

    PCFieldController pcFieldController => PCFieldController.instance;

    GameObject electricShockObj;
    List<ElectricShock> electricShocks = new List<ElectricShock>();

    // Start is called before the first frame update
    void Start()
    {
        holdDisplay = GetComponent<PlayerAttack>().holdDisplay;

        electricShockObj = pcFieldController.electricShock;
        GameObject electricShockPool = new GameObject("ElectricShockPool");
        for (int i = 0; i < 10; i++)
        {
            GameObject electric = Instantiate(electricShockObj, new Vector3(0, 95, 0), Quaternion.identity);
            ElectricShock ele = electric.GetComponent<ElectricShock>();
            electric.transform.parent = electricShockPool.transform;
            electricShocks.Add(ele);
        }
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

            if (element == Element.Thunder)
            {
                for (int i = 0; i < electricShocks.Count; i++)
                {
                    if (electricShocks[i].isSleep) 
                    {
                        electricShocks[i].Initialize(selectEnemy);
                        break;
                    }
                }
            }
        }

    }

    public Element GetElementKind(Range range)
    {
        Element element = Element.Thunder;
        return element;
    }

    public float GetMaxLength()
    {
        return MAX_LENGTH;
    }
}
