using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TxtTallet : MonoBehaviour ,IGrabbable
{

    Transform talletTrans;
    SpriteRenderer renderer;
    bool isGrabed;
    bool isSleep;

    Vector3 diffPlayerVec = Vector2.zero;
    public PCFieldController pcFieldController;

    const float TALLET_INTERVAL = 3f;

    [SerializeField]float talletTimer = 0;

    [SerializeField] TxtBullet[] txtBullets;

    RangeCheck rangeCheck;

    // Start is called before the first frame update
    void Start()
    {
        rangeCheck = GetComponentInChildren<RangeCheck>();
        talletTrans = GetComponent<Transform>();
        renderer = GetComponent<SpriteRenderer>();
        Putting();
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSleep) 
        {
            //すでにインターバルを超えていたら時間は追加しない
            if (talletTimer < TALLET_INTERVAL)
            {
                talletTimer += Time.deltaTime;
            }

            if (rangeCheck.nearEnemy != null && !isGrabed)
            {
                if (talletTimer >= TALLET_INTERVAL)
                {
                    Shot();
                    talletTimer = 0;
                }
            }
        }
        
    }

    void Shot() 
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
    }

    public void Initialize()
    {
        isSleep = false;
        renderer.enabled = true;
    }
}
