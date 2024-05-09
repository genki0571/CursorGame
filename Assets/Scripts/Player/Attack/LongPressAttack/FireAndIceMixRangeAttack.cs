using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAndIceMixRangeAttack : MonoBehaviour,IHoldAttacker
{
    PCFieldController pcFieldController => PCFieldController.instance;
    List<Explotion> explotions = new List<Explotion>();
    GameObject fireAreaObj;
    List<FireArea> fireAreas = new List<FireArea>();

    const float FIRE_DAMAGE = 10;
    const float ICE_DAMAGE = 5;

    const float MAX_LENGTH = 5;

    GameObject holdDisplay;

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

    public void Attack(List<GameObject> selectEnemies, Range range)
    {
        Element element = GetElementKind(range);
        if (element == Element.Fire)
        {
            for (int i = 0; i < selectEnemies.Count; i++)
            {
                IDamagable damagable = selectEnemies[i].GetComponent<IDamagable>();

                if (damagable != null)
                {
                    damagable.AddDamage(FIRE_DAMAGE);
                    damagable.AddElement(Element.Fire, holdDisplay.transform.position);
                }
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
        else if (element == Element.Ice)
        {
            for (int i = 0; i < selectEnemies.Count; i++)
            {
                IDamagable damagable = selectEnemies[i].GetComponent<IDamagable>();

                if (damagable != null)
                {
                    damagable.AddDamage(ICE_DAMAGE);
                    damagable.AddElement(Element.Ice, holdDisplay.transform.position);
                }
            }

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

    public float GetMaxLength()
    {
        return MAX_LENGTH;
    }
}
