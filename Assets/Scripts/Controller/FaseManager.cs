using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseManager : MonoBehaviour
{
    //この中に必要な敵のPrefabsを入れる
    [SerializeField] int faseNum;
    [SerializeField] public GameObject[] faseEnemyPrefabs;
    [System.NonSerialized] public GameObject[] faseEnemies;
    [System.NonSerialized] public EnemyBase[] faseEnemyBases;

    [SerializeField] public float minPopInterval;
    [SerializeField] public float maxPopInterval;

    private void Awake()
    {
        faseEnemies = new GameObject[faseEnemyPrefabs.Length];
        faseEnemyBases = new EnemyBase[faseEnemyPrefabs.Length];
        GameObject faseObjPool = new GameObject("Fase" + faseNum + "EnemyPool");
        for (int i = 0; i < faseEnemies.Length; i++)
        {
            GameObject enemy = Instantiate(faseEnemyPrefabs[i], transform.position, Quaternion.identity);
            enemy.transform.parent = faseObjPool.transform;
            faseEnemies[i] = enemy;
            faseEnemyBases[i] = enemy.GetComponent<EnemyBase>();
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
