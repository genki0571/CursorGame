using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TxtTallet : MonoBehaviour ,IGrabbable
{

    Transform talletTrans;
    SpriteRenderer renderer;
    bool isGrabed;
    public bool isSleep;

    Vector3 diffPlayerVec = Vector2.zero;
    PCFieldController pcFieldController => PCFieldController.instance;

    const float TALLET_ATTACK_INTERVAL = 3f;
    [SerializeField]float attackTimer = 0;

    const float TALLET_MAX_LIFE = 30;
    const float TALLET_LIFE_COST_PER_S = 1;
    [SerializeField] float talletLife = 0;

    TxtBullet[] txtBullets;

    RangeCheck rangeCheck;

    // Start is called before the first frame update
    void Start()
    {
        rangeCheck = GetComponentInChildren<RangeCheck>();
        talletTrans = GetComponent<Transform>();
        renderer = GetComponent<SpriteRenderer>();
        txtBullets = GetComponentsInChildren<TxtBullet>();
        Putting();
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSleep) 
        {
            talletLife -= TALLET_LIFE_COST_PER_S * Time.deltaTime;
            if (talletLife <= 0) 
            {
                Reset();
            }

            //すでにインターバルを超えていたら時間は追加しない
            if (attackTimer < TALLET_ATTACK_INTERVAL)
            {
                attackTimer += Time.deltaTime;
            }

            if (rangeCheck.nearEnemy != null && !isGrabed)
            {
                if (attackTimer >= TALLET_ATTACK_INTERVAL)
                {
                    Shot();
                    attackTimer = 0;
                }
            }
        }
        
    }

    private void Shot() 
    {
        Debug.Log("SHOT");

        TxtBullet bullet = null;
        for (int i = 0; i < txtBullets.Length; i++)
        {
            if (txtBullets[i].isSleep) 
            {
                bullet = txtBullets[i];
            }
        }

        if (bullet != null) 
        {
            bullet.Initialize(rangeCheck.nearEnemy);
        }
    }

    public void Grabbing(Transform cursorTrans)
    {
        if (!isGrabed)
        {
            diffPlayerVec = talletTrans.position - cursorTrans.position;
            pcFieldController.MovePos(this.gameObject);
        }
        talletTrans.position = cursorTrans.position + diffPlayerVec;
        isGrabed = true;
    }

    public void Putting()
    {
        isGrabed = false;

        talletTrans.position = pcFieldController.GetPos(this.gameObject, talletTrans.position);
    }

    void Reset() 
    {
        isSleep = true;
        renderer.enabled = false;
        talletLife = TALLET_MAX_LIFE;
        pcFieldController.MovePos(this.gameObject);
    }

    public void Initialize(Vector3 pos)
    {
        talletTrans.position = pos;
        isSleep = false;
        renderer.enabled = true;
        Putting();
    }
}
