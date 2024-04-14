using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public static EnemyGenerator instance;

    PhaseManager[] phaseManagers;

    public int phaseNum = 0;
    public int phaseEnemyNum;
    int endPhaseNum;
    bool popedSponer;

    private float popTimer;
    Vector2 cameraLeftLowPos;
    Vector2 cameraRightUpperPos;
    Vector2 leftLowPopPos;
    Vector2 rightUpperPopPos;

    [System.NonSerialized] public List<GameObject> allEnemies = new List<GameObject>();
    [System.NonSerialized] public List<EnemyBase> allEnemyBase = new List<EnemyBase>();

    [System.NonSerialized] public List<GameObject> activeEnemies = new List<GameObject>();

    [SerializeField] GameObject SponeDirObjUpper;
    [SerializeField] GameObject SponeDirObjLower;
    [SerializeField] GameObject SponeDirObjLeft;
    [SerializeField] GameObject SponeDirObjRight;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraLeftLowPos = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        cameraRightUpperPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        leftLowPopPos = cameraLeftLowPos + new Vector2(-1,-1);
        rightUpperPopPos = cameraRightUpperPos + new Vector2(1,1);


        phaseManagers = GetComponentsInChildren<PhaseManager>();
        endPhaseNum = phaseManagers.Length - 1;

        for (int i = 0; i < phaseManagers.Length; i++)
        {
            for (int j = 0; j < phaseManagers[i].phaseEnemies.Length; j++)
            {
                allEnemies.Add(phaseManagers[i].phaseEnemies[j]);
                allEnemyBase.Add(phaseManagers[i].phaseEnemies[j].GetComponent<EnemyBase>());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        activeEnemies.Clear();
        for (int i = 0; i < allEnemyBase.Count; i++)
        {
            if (allEnemyBase[i].state != EnemyBase.State.Sleep)
            {
                activeEnemies.Add(allEnemies[i]);
            }
        }


        popTimer += Time.deltaTime;

        if (phaseNum < phaseManagers.Length)
        {
            if (phaseEnemyNum < phaseManagers[phaseNum].phaseEnemies.Length)
            {
                if (!popedSponer)
                {
                    for (int j = 0; j < phaseManagers[phaseNum].sponers.Length; j++)
                    {
                        phaseManagers[phaseNum].sponers[j].Initialize(phaseManagers[phaseNum].sponerPos[j]);
                    }
                    popedSponer = true;
                }

                if (popTimer >= phaseManagers[phaseNum].GetPopInterval())
                {
                    phaseManagers[phaseNum].phaseEnemyBases[phaseEnemyNum].Initialize();
                    phaseManagers[phaseNum].phaseEnemies[phaseEnemyNum].transform.position = GetPopPos();
                    popTimer = 0;
                    phaseEnemyNum += 1;
                }
            }
        }
        if (phaseNum < phaseManagers.Length)
        {
            if (phaseEnemyNum >= phaseManagers[phaseNum].phaseEnemyBases.Length)
            {
                for (int i = 0; i < phaseManagers[phaseNum].phaseEnemyBases.Length; i++)
                {
                    if (phaseManagers[phaseNum].phaseEnemyBases[i].state == EnemyBase.State.Sleep)
                    {

                    }
                    else
                    {
                        break;
                    }

                    if (i == phaseManagers[phaseNum].phaseEnemyBases.Length - 1)
                    {
                        //次のフェーズへ進む
                        phaseNum += 1;
                        phaseEnemyNum = 0;
                        popedSponer = false;
                        break;
                    }
                }
            }
        }

        ShowSponeDir();

        if (phaseNum > endPhaseNum) 
        {
            Debug.Log("フェーズ完了");
        }
    }

    private Vector3 GetPopPos()
    {
        Vector3 popPos = Vector3.zero;
        int maxNum = 0;
        if (phaseManagers[phaseNum].canSponeUpper)
        {
            maxNum += 1;
        }
        if (phaseManagers[phaseNum].canUpperRight)
        {
            maxNum += 1;
        }
        if (phaseManagers[phaseNum].canSponeLower)
        {
            maxNum += 1;
        }
        if (phaseManagers[phaseNum].canUpperLeft)
        {
            maxNum += 1;
        }
        int rand = Random.Range(0,maxNum);
        int num = 0;

        int sponePointNum = 0;
        if (phaseManagers[phaseNum].canSponeUpper)
        {
            num++;
            if (num <= maxNum && rand == num - 1) 
            {
                sponePointNum = 0;
            }
        }
        if (phaseManagers[phaseNum].canUpperRight)
        {
            num++;
            if (num <= maxNum && rand == num - 1)
            {
                sponePointNum = 1;
            }
        }
        if (phaseManagers[phaseNum].canSponeLower)
        {
            num++;
            if (num <= maxNum && rand == num - 1)
            {
                sponePointNum = 2;
            }
        }
        if (phaseManagers[phaseNum].canUpperLeft)
        {
            num++;
            if (num <= maxNum && rand == num - 1)
            {
                sponePointNum = 3;
            }
        }

        switch (sponePointNum)
        {
            case 0://上から
                if (phaseManagers[phaseNum].canSponeUpper)
                {
                    popPos.y = rightUpperPopPos.y;
                    popPos.x = (Random.Range(leftLowPopPos.x, rightUpperPopPos.x));
                }
                break;
            case 1://右から
                if (phaseManagers[phaseNum].canUpperRight)
                {
                    popPos.x = rightUpperPopPos.x;
                    popPos.y = (Random.Range(leftLowPopPos.y, rightUpperPopPos.y));
                }
                break;
            case 2://下から
                if (phaseManagers[phaseNum].canSponeLower)
                {
                    popPos.y = leftLowPopPos.y;
                    popPos.x = (Random.Range(leftLowPopPos.x, rightUpperPopPos.x));
                }
                break;
            case 3://左から
                if (phaseManagers[phaseNum].canUpperLeft)
                {
                    popPos.x = leftLowPopPos.x;
                    popPos.y = (Random.Range(leftLowPopPos.y, rightUpperPopPos.y));
                }
                break;
        }

        return popPos;
    }

    void ShowSponeDir() 
    {
        if (SponeDirObjUpper != null) 
        {
            if (phaseNum < phaseManagers.Length)
            {
                PhaseManager phase = phaseManagers[phaseNum];
                SponeDirObjUpper.SetActive(phase.canSponeUpper);
                SponeDirObjLower.SetActive(phase.canSponeLower);
                SponeDirObjLeft.SetActive(phase.canUpperLeft);
                SponeDirObjRight.SetActive(phase.canUpperRight);
            }
        }
    }
}
