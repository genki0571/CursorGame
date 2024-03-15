using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    //この中に必要な敵のPrefabsを入れる
    [SerializeField] int phaseNum;
    [SerializeField] public GameObject[] phaseEnemyPrefabs;
    [System.NonSerialized] public GameObject[] phaseEnemies;
    [System.NonSerialized] public EnemyBase[] phaseEnemyBases;

    [SerializeField] public float minPopInterval;
    [SerializeField] public float maxPopInterval;

    private void Awake()
    {
        phaseEnemies = new GameObject[phaseEnemyPrefabs.Length];
        phaseEnemyBases = new EnemyBase[phaseEnemyPrefabs.Length];
        GameObject phaseObjPool = new GameObject("Phase" + phaseNum + "EnemyPool");
        for (int i = 0; i < phaseEnemies.Length; i++)
        {
            GameObject enemy = Instantiate(phaseEnemyPrefabs[i], transform.position, Quaternion.identity);
            enemy.transform.parent = phaseObjPool.transform;
            phaseEnemies[i] = enemy;
            phaseEnemyBases[i] = enemy.GetComponent<EnemyBase>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetPopInterval() 
    {
        return Random.Range(minPopInterval, maxPopInterval);
    }
}
