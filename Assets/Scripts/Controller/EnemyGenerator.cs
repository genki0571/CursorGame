using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public static EnemyGenerator instance;

    FaseManager[] faseManagers;

    public int faseNum = 0;
    public int faseEnemyNum;
    int endFaseNum;

    private float popTimer;
    Vector2 cameraLeftLowPos;
    Vector2 cameraRightUpperPos;
    Vector2 leftLowPopPos;
    Vector2 rightUpperPopPos;

    [System.NonSerialized] public List<GameObject> allEnemies = new List<GameObject>();
    [System.NonSerialized] public List<EnemyBase> allEnemyBase = new List<EnemyBase>();

    [System.NonSerialized] public List<GameObject> activeEnemies = new List<GameObject>();

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


        faseManagers = GetComponentsInChildren<FaseManager>();
        endFaseNum = faseManagers.Length - 1;

        for (int i = 0; i < faseManagers.Length; i++)
        {
            for (int j = 0; j < faseManagers[i].faseEnemies.Length; j++)
            {
                allEnemies.Add(faseManagers[i].faseEnemies[j]);
                allEnemyBase.Add(faseManagers[i].faseEnemies[j].GetComponent<EnemyBase>());
            }
        }

        for (int i = 0; i < allEnemyBase.Count; i++)
        {
            allEnemyBase[i].Reset();
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

        if (faseNum < faseManagers.Length)
        {
            if (faseEnemyNum < faseManagers[faseNum].faseEnemies.Length)
            {
                if (popTimer >= faseManagers[faseNum].GetPopInterval())
                {
                    faseManagers[faseNum].faseEnemyBases[faseEnemyNum].Initialize();
                    faseManagers[faseNum].faseEnemies[faseEnemyNum].transform.position = GetPopPos();
                    popTimer = 0;
                    faseEnemyNum += 1;
                }
            }
        }

        if (faseEnemyNum >= faseManagers[faseNum].faseEnemies.Length)
        {
            if (faseNum < faseManagers.Length)
            {
                for (int i = 0; i < faseManagers[faseNum].faseEnemyBases.Length; i++)
                {
                    if (faseManagers[faseNum].faseEnemyBases[i].state == EnemyBase.State.Sleep)
                    {

                    }
                    else
                    {
                        break;
                    }

                    if (i == faseManagers[faseNum].faseEnemyBases.Length - 1)
                    {
                        //次のフェーズへ進む
                        faseNum += 1;
                    }
                }
            }
        }
        

        if (faseNum > endFaseNum) 
        {
            Debug.Log("フェーズ完了");
        }
    }

    private Vector3 GetPopPos()
    {
        Vector3 popPos = Vector3.zero;
        int rand = Random.Range(0,4);
        switch (rand)
        {
            case 0://上から
                popPos.y = rightUpperPopPos.y;
                popPos.x = (Random.Range(leftLowPopPos.x, rightUpperPopPos.x));
                break;
            case 1://右から
                popPos.x = rightUpperPopPos.x;
                popPos.y = (Random.Range(leftLowPopPos.y, rightUpperPopPos.y));
                break;
            case 2://下から
                popPos.y = leftLowPopPos.y;
                popPos.x = (Random.Range(leftLowPopPos.x, rightUpperPopPos.x));
                break;
            case 3://左から
                popPos.x = leftLowPopPos.x;
                popPos.y = (Random.Range(leftLowPopPos.y, rightUpperPopPos.y));
                break;
        }

        return popPos;
    }
}
