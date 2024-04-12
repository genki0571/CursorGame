using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanWeakPoint : MonoBehaviour,IRightAttacker
{
    EnemyGenerator enemyGenerator => EnemyGenerator.instance;
    PCFieldController pcFieldController => PCFieldController.instance;
         
    Command command = global::Command.Scan;

    [SerializeField] List<GameObject> activeEnemies = new List<GameObject>();
    List<WeekPoint> weekPoints = new List<WeekPoint>();
   
    bool isScanning;
    const float SCAN_INTERVAL = 3f;

    // Start is called before the first frame update
    void Start()
    {
        activeEnemies = enemyGenerator.activeEnemies;
        weekPoints = pcFieldController.weekPoints;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Command(Vector3 pos, List<ISelectable> selectDamagables)
    {
        isScanning = true;
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            if (activeEnemies[i] != null)
            {
                IHaveWeakPoint haveWeakPoint = activeEnemies[i].GetComponent<IHaveWeakPoint>();
                if (haveWeakPoint != null)
                {
                    for (int j = 0; j < weekPoints.Count; j++)
                    {
                        if (weekPoints[j].isSleep) 
                        {
                            weekPoints[j].ShowPoint(haveWeakPoint, SCAN_INTERVAL);
                            break;
                        }
                    }
                }
            }
        }
    }

    public Command GetCommandKind()
    {
        return command;
    }
}
