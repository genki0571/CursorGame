using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUIGenerator : MonoBehaviour
{
    static public EnemyUIGenerator instance;

    [SerializeField] GameObject damageDisplayObj;
    public List<DamageDisplay> damageDisplays = new List<DamageDisplay>();
    GameObject damageDisplayPool;

    float timer = 0;

    bool addable = false;

    void Awake()
    {
        instance = this;

        damageDisplayPool = new GameObject("EnemyDamageDisplayPool");
        for (int i = 0; i < 10; i++)
        {
            GameObject display = Instantiate(damageDisplayObj,new Vector3(0,80,0),Quaternion.identity);
            display.transform.parent = damageDisplayPool.transform;
            damageDisplays.Add(display.GetComponent<DamageDisplay>());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (damageDisplays.Count <= 40) 
        {
            timer += Time.deltaTime;
            if (timer >= 3) 
            {
                GameObject displayObj = Instantiate(damageDisplayObj, new Vector3(0, 80, 0), Quaternion.identity);
                displayObj.transform.parent = damageDisplayPool.transform;
                DamageDisplay display = displayObj.GetComponent<DamageDisplay>();
                damageDisplays.Add(display);
                timer = 0;
            }
        }
    }
}
