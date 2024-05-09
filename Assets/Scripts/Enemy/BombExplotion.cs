using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplotion : MonoBehaviour
{
    PlayerState playerState => PlayerState.instance;
    Transform playerTrans;
    Transform explotionTrans;
    SpriteRenderer renderer;

    float radiusS = 2;
    float radiusM = 3;
    float radiusL = 4;

    float intervalS = 3;
    float intervalM = 2;
    float intervalL = 1;

    public bool isSleep;

    int explotionCnt = 0;
    const int MAX_CNT = 6;
    float resetTimer = 0;
    const float RESET_INTERVAL = 0.5f;

    RangeCheck rangeCheck;
    List<GameObject> withinEnemys;

    // Start is called before the first frame update
    void Start()
    {
        playerTrans = playerState.transform;
        explotionTrans = this.transform;
        renderer = GetComponent<SpriteRenderer>();
        rangeCheck = GetComponent<RangeCheck>();
        withinEnemys = rangeCheck.withinDamegableObjects;

        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSleep) 
        {
            if (explotionCnt++ == MAX_CNT)
            {
                Explotion();
            }

            resetTimer += Time.deltaTime;
            if (resetTimer >= RESET_INTERVAL)
            {
                Reset();
            }
        }
    }

    private void Reset()
    {
        renderer.enabled = false;
        isSleep = true;
    }

    public void Initialize(Vector3 pos,float radius, float interval)
    {
        explotionTrans.position = pos;

        radiusS = radius * 1;
        radiusM = radius * 2;
        radiusL = radius * 3;
        intervalS = interval / 1;
        intervalM = interval / 2;
        intervalL = interval / 3;

        renderer.enabled = true;
        explotionTrans.localScale = new Vector3(radiusL * 2,radiusL * 2,radiusL * 2);

        isSleep = false;
        resetTimer = 0;
        explotionCnt = 0;
    }

    private void Explotion() 
    {
        float sqrDistance = (playerTrans.position - explotionTrans.position).sqrMagnitude;
        if (sqrDistance <= radiusS * radiusS)
        {
            playerState.AddDamage(1);
            playerState.GetStan(intervalS);
        }
        else if (sqrDistance <= radiusM * radiusM)
        {
            playerState.AddDamage(1);
            playerState.GetStan(intervalM);
        }
        else if (sqrDistance <= radiusL * radiusL)
        {
            playerState.AddDamage(1);
            playerState.GetStan(intervalL);
        }

        for (int i = 0; i < withinEnemys.Count; i++)
        {
            IDamagable damagable = withinEnemys[i].GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.AddDamage(50);
            }
        }
    }
}
