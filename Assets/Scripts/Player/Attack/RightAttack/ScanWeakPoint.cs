using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanWeakPoint : MonoBehaviour,IRightAttacker
{
    EnemyGenerator enemyGenerator => EnemyGenerator.instance;
    PCFieldController pcFieldController => PCFieldController.instance;
         
    Command command = global::Command.Scan;

    [SerializeField] List<GameObject> activeEnemies = new List<GameObject>();
    List<Transform> targetObjs = new List<Transform>();
    GameObject targetsPool;
    GameObject targetObj;

    bool isScanning;
    const float SCAN_INTERVAL = 3f;
    float scanTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        activeEnemies = enemyGenerator.activeEnemies;
        targetObj = pcFieldController.scanTargetObj;
        targetsPool = new GameObject("TargetsPool");
        for (int i = 0; i < 30; i++)
        {
            GameObject target1 = Instantiate(targetObj, new Vector3(0,100,0),Quaternion.identity);
            Transform targetTrans = target1.transform;
            targetTrans.parent = targetsPool.transform;
            targetObjs.Add(targetTrans);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (isScanning)
        {
            if (scanTimer <= SCAN_INTERVAL)
            {
                scanTimer += Time.deltaTime;
            }
            if (scanTimer >= SCAN_INTERVAL) 
            {
                scanTimer = 0;
                isScanning = false;

                for (int i = 0; i < targetObjs.Count; i++)
                {
                    if (targetObjs[i].position.y != 100)
                    {
                        targetObjs[i].position = new Vector3(0,100,0);
                    }
                }
            }
            else 
            {
                int num = 0;
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    if (activeEnemies[i] != null)
                    {
                        IHaveWeakPoint haveWeakPoint = activeEnemies[i].GetComponent<IHaveWeakPoint>();
                        if (haveWeakPoint != null)
                        {
                            targetObjs[num].position = haveWeakPoint.GetWeekPoint();
                            num++;
                        }
                    }
                }
            }
        }
        else 
        {
            
            scanTimer = 0;
        }
    }

    public void Command(Vector3 pos, List<ISelectable> selectDamagables)
    {
        isScanning = true;
    }

    public Command GetCommandKind()
    {
        return command;
    }
}
