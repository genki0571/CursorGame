using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCheck : MonoBehaviour
{
    Transform trans;

    List<IDamagable> withinDamegables = new List<IDamagable>();
    List<GameObject> withinDamegableObjects = new List<GameObject>();
    public GameObject nearEnemy;

    // Start is called before the first frame update
    void Start()
    {
        trans = this.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < withinDamegableObjects.Count; i++)
        {
            if (!withinDamegableObjects[i])
            {
                withinDamegables.RemoveAt(i);
                withinDamegableObjects.RemoveAt(i);
            }
        }

        nearEnemy = null;
        float minDistanceSqr = 10000;
        for (int i = 0; i < withinDamegables.Count; i++)
        {
            float distanceSqr = (withinDamegableObjects[i].transform.position - trans.position).sqrMagnitude;

            if (distanceSqr <= minDistanceSqr)
            {
                minDistanceSqr = distanceSqr;
                nearEnemy = withinDamegableObjects[i];
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable damagable = collision.GetComponent<IDamagable>();
        if (damagable != null) 
        {
            bool already = false;
            for (int i = 0; i < withinDamegables.Count; i++)
            {
                if (damagable == withinDamegables[i]) 
                {
                    already = true;
                    break;
                }
            }

            if (!already) 
            {
                withinDamegables.Add(damagable);
                withinDamegableObjects.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IDamagable damagable = collision.GetComponent<IDamagable>();
        if (damagable != null)
        {
            for (int i = 0; i < withinDamegables.Count; i++)
            {
                if (damagable == withinDamegables[i])
                {
                    withinDamegables.RemoveAt(i);
                    withinDamegableObjects.RemoveAt(i);
                }
            }
        }
    }
}
