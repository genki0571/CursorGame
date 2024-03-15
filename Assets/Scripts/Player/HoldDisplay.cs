using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldDisplay : MonoBehaviour
{
    public List<IDamagable> withinRangeDamagable = new List<IDamagable>();
    public List<GameObject> withinRangeEnemies = new List<GameObject>();
    [SerializeField] PlayerAttack playerAttack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamagable damagable = other.transform.GetComponent<IDamagable>();
        if (damagable != null) 
        {
            bool already = false;
            for (int i = 0; i < withinRangeDamagable.Count; i++)
            {
                if (damagable == withinRangeDamagable[i]) 
                {
                    already = true;
                }
            }

            if (!already)
            {
                withinRangeDamagable.Add(damagable);
                withinRangeEnemies.Add(other.transform.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IDamagable damagable = other.transform.GetComponent<IDamagable>();
        if (damagable != null)
        {
            for (int i = 0; i < withinRangeDamagable.Count; i++)
            {
                if (damagable == withinRangeDamagable[i]) 
                {
                    withinRangeDamagable.RemoveAt(i);
                    withinRangeEnemies.RemoveAt(i);
                }
            }
        }
    }
}
