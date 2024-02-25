using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongPressDisplay : MonoBehaviour
{
    public List<IDamagable> withinRangeEnemies = new List<IDamagable>();
    [SerializeField] Player player;

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
            for (int i = 0; i < withinRangeEnemies.Count; i++)
            {
                if (damagable == withinRangeEnemies[i]) 
                {
                    already = true;
                }
            }

            if (!already)
            {
                withinRangeEnemies.Add(damagable);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IDamagable damagable = other.transform.GetComponent<IDamagable>();
        if (damagable != null)
        {
            for (int i = 0; i < withinRangeEnemies.Count; i++)
            {
                if (damagable == withinRangeEnemies[i]) 
                {
                    withinRangeEnemies.RemoveAt(i);
                }
            }
        }
    }
}
