using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUIGenerator : MonoBehaviour
{
    static public EnemyUIGenerator instance;

    [SerializeField] GameObject damageDisplayPool;
    public List<DamageDisplay> damageDisplays = new List<DamageDisplay>();

    void Awake()
    {
        instance = this;

        DamageDisplay[] dmgs = damageDisplayPool.GetComponentsInChildren<DamageDisplay>();
        for (int i = 0; i < dmgs.Length; i++)
        {
            damageDisplays.Add(dmgs[i]);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
