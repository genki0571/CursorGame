using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PngBuster : MonoBehaviour,IGrabbable
{
    Rigidbody2D rb;

    Transform pngBusterTrans;
    PCFieldController pcFieldController => PCFieldController.instance;

    bool isGrabed = false;
    Vector3 diffPlayerVec;


    public bool isSleep;
    SpriteRenderer renderer;

    const float BUSTER_SPEED = 5;
    const float BUSTER_DAMAGE = 15;

    const float BUSTER_ATTACK_INTERVAL = 2;
    float attackTimer = 0;
    const float BUSTER_ATTACK_DISTANCE_SQR = 1;
    Vector3 pngBusterVelocity;
    RangeCheck  rangeCheck;

    const float BUSTER_MAX_LIFE = 30;
    const float BUSTER_LIFE_COST_PER_S = 1;
    float busterLife = 0;

    // Start is called before the first frame update
    void Start()
    {
        rangeCheck = GetComponentInChildren<RangeCheck>();
        rb = GetComponent<Rigidbody2D>();
        pngBusterTrans = GetComponent<Transform>();
        renderer = GetComponent<SpriteRenderer>();
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSleep)
        {
            busterLife -= BUSTER_LIFE_COST_PER_S * Time.deltaTime;
            if (busterLife <= 0)
            {
                Reset();
            }

            //すでにインターバルを超えていたら時間は追加しない
            if (attackTimer < BUSTER_ATTACK_INTERVAL)
            {
                attackTimer += Time.deltaTime;
            }

            if (rangeCheck.nearEnemy != null && !isGrabed)
            {
                //近づく
                Vector3 enemyDiffVec = (rangeCheck.nearEnemy.transform.position - pngBusterTrans.position);
                Vector3 enemyDir = enemyDiffVec.normalized;
                pngBusterVelocity = enemyDir * BUSTER_SPEED;

                if (enemyDiffVec.sqrMagnitude <= BUSTER_ATTACK_DISTANCE_SQR)
                {
                    if (attackTimer >= BUSTER_ATTACK_INTERVAL)
                    {
                        Attack(rangeCheck.nearEnemy);
                        attackTimer = 0;
                    }

                    pngBusterVelocity = Vector3.zero;
                }
            }
            else 
            {
                pngBusterVelocity = Vector3.zero;
            }
        }
        else 
        {
            pngBusterVelocity = Vector3.zero;
        }

        rb.velocity = pngBusterVelocity;
    }

    private void Attack(GameObject attackEnemy) 
    {
        IDamagable damagable = attackEnemy.GetComponent<IDamagable>();
        if (damagable != null) 
        {
            damagable.AddDamage(BUSTER_DAMAGE);
        }
    }


    public void Grabbing(Transform cursorTrans)
    {
        if (!isGrabed)
        {
            diffPlayerVec = pngBusterTrans.position - cursorTrans.position;
        }
        pngBusterTrans.position = cursorTrans.position + diffPlayerVec;
        isGrabed = true;
    }

    public void Putting()
    {
        isGrabed = false;
    }

    void Reset()
    {
        isSleep = true;
        renderer.enabled = false;
        busterLife = BUSTER_MAX_LIFE;
    }

    public void Initialize(Vector3 pos)
    {
        pngBusterTrans.position = pos;
        isSleep = false;
        renderer.enabled = true;
    }
}
