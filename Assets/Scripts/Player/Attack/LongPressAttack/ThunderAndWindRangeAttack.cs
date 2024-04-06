using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderAndWindRangeAttack : MonoBehaviour,IHoldAttacker
{
    const float THUNDER_DAMAGE = 10;
    const float WIND_DAMAGE = 0;

    const float MAX_LENGTH = 5;

    GameObject holdDisplay;

    PCFieldController pcFieldController => PCFieldController.instance;
    GameObject electricShockObj;
    List<ElectricShock> electricShocks = new List<ElectricShock>();
    GameObject windAreaObj;
    List<WindArea> windAreas = new List<WindArea>();

    // Start is called before the first frame update
    void Start()
    {
        holdDisplay = GetComponent<PlayerAttack>().holdDisplay;
        windAreaObj = pcFieldController.windArea;
        GameObject windAreaPool = new GameObject("WindAreaPool");

        electricShockObj = pcFieldController.electricShock;
        GameObject electricShockPool = new GameObject("ElectricShockPool");
        for (int i = 0; i < 10; i++)
        {
            GameObject electric = Instantiate(electricShockObj, new Vector3(0, 95, 0), Quaternion.identity);
            ElectricShock ele = electric.GetComponent<ElectricShock>();
            electric.transform.parent = electricShockPool.transform;
            electricShocks.Add(ele);
        }

        for (int i = 0; i < 3; i++)
        {
            GameObject area = Instantiate(windAreaObj,new Vector3(0,95,0),Quaternion.identity);
            WindArea windArea = area.GetComponent<WindArea>();
            windAreas.Add(windArea);
            area.transform.parent = windAreaPool.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Attack(GameObject selectEnemy, Range range)
    {
        IDamagable damagable = null;
        Element element = GetElementKind(range);
        if (selectEnemy != null)
        {
            damagable = selectEnemy.GetComponent<IDamagable>();
        }

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
        else if (element == Element.Wind)
        {
            for (int i = 0; i < windAreas.Count; i++)
            {
                if (windAreas[i].isSleep)
                {
                    windAreas[i].Initialize(holdDisplay.transform);
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
            element = Element.Thunder;
        }
        else if (range == Range.Left)
        {
            element = Element.Wind;
        }
        else if (range == Range.Mix)
        {
            element = Element.Empty;
        }
        return element;
    }

    public float GetMaxLength()
    {
        return MAX_LENGTH;
    }
}
