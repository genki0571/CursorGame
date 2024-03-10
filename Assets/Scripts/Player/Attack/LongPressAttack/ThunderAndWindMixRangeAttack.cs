using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderAndWindMixRangeAttack : MonoBehaviour,ILongPressAttacker
{
    const float THUNDER_DAMAGE = 10;
    const float WIND_DAMAGE = 0;

    GameObject longPressDisplay;
    Player player;


    PCFieldController pcFieldController => PCFieldController.instance;
    GameObject windAreaObj;
    List<WindArea> windAreas = new List<WindArea>();
    GameObject stormObj;
    List<Storm> storms = new List<Storm>();

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        longPressDisplay = player.longPressDisplay;

        windAreaObj = pcFieldController.windArea;
        stormObj = pcFieldController.storm;
        GameObject windAreaPool = new GameObject("WindAreaPool");
        GameObject stormPool = new GameObject("StormPool");
        for (int i = 0; i < 1; i++)
        {
            GameObject area = Instantiate(windAreaObj, new Vector3(0, 95, 0), Quaternion.identity);
            WindArea windArea = area.GetComponent<WindArea>();
            windAreas.Add(windArea);
            area.transform.parent = windAreaPool.transform;

            GameObject sto = Instantiate(stormObj,new Vector3(1,95,0),Quaternion.identity);
            Storm s = sto.GetComponent<Storm>();
            storms.Add(s);
            sto.transform.parent = stormPool.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Attack(GameObject selectEnemy, Range range)
    {
        IDamagable damagable = null ;
        Element element = GetElementKind(range);
        if (selectEnemy != null)
        {
            damagable = selectEnemy.GetComponent<IDamagable>();
        }

        float damage = 0;
        if (element == Element.Thunder)
        {
            if (damagable != null)
            {
                damagable.AddElement(Element.Thunder, longPressDisplay.transform.position);
                damage = THUNDER_DAMAGE;
                damagable.AddDamage(damage);
            }
        }
        else if (element == Element.Wind)
        {
            for (int i = 0; i < windAreas.Count; i++)
            {
                if (windAreas[i].isSleep)
                {
                    windAreas[i].Initialize(longPressDisplay.transform);
                    break;
                }
            }
        }
        else if (element == Element.ThunderAndWind)
        {
            for (int i = 0; i < storms.Count; i++)
            {
                if (storms[i].isSleep)
                {
                    storms[i].Initialize(player.longPressStartPos);
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
            element = Element.ThunderAndWind;
        }
        return element;
    }
}
