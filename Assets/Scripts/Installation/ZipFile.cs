using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZipFile : MonoBehaviour, IGrabbable
{
    Transform ZipTrans;
    SpriteRenderer renderer;
    bool isGrabed;
    public bool isSleep;

    Vector3 diffPlayerVec = Vector3.zero;

    bool isOpen;

    const float ZIP_INTERVAL = 5;
    float zipLifeTimer = 0;

    const float ZIP_ATTACK_INTERVAL = 0.5f;
    float zipAttackTimer = 0;

    const float OPEN_INTERVAL = 5;
    float openTimer = 0;

    PCFieldController pcFieldController => PCFieldController.instance;
    [SerializeField] Text zipTimerText;
    [SerializeField] AnimationCurve curve;

    DataFileBullet[] dataFileBullets;


    // Start is called before the first frame update
    void Start()
    {
        renderer =GetComponent<SpriteRenderer>();
        ZipTrans = GetComponent<Transform>();
        dataFileBullets = GetComponentsInChildren<DataFileBullet>();
        Putting();
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSleep)
        {
            if (!isOpen) 
            {
                float loadAmount = curve.Evaluate(zipLifeTimer);
                string loadAmountStr = loadAmount.ToString("f1");
                zipTimerText.text = "" + loadAmountStr + "％";

                //時間計測
                if (zipLifeTimer <= ZIP_INTERVAL)
                {
                    zipLifeTimer += Time.deltaTime;
                }
                if (zipLifeTimer >= ZIP_INTERVAL)
                {
                    isOpen = true;
                }

            }

            if (isOpen) 
            {
                zipTimerText.text = "展開";

                //時間計測
                if (zipAttackTimer <= ZIP_ATTACK_INTERVAL)
                {
                    zipAttackTimer += Time.deltaTime;
                }
                if (zipAttackTimer >= ZIP_ATTACK_INTERVAL)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector3 dir = Vector3.zero;
                        switch (i) 
                        {
                            case 0:
                                dir = new Vector3(1, 0, 0);
                                break;
                            case 1:
                                dir = new Vector3(0, 1, 0);
                                break;
                            case 2:
                                dir = new Vector3(-1, 0, 0);
                                break;
                            case 3:
                                dir = new Vector3(0, -1, 0);
                                break;
                        }

                        Attack(dir);
                    }

                    zipAttackTimer = 0;
                }

                //時間計測
                if (openTimer <= OPEN_INTERVAL)
                {
                    openTimer += Time.deltaTime;
                }
                if (openTimer >= OPEN_INTERVAL)
                {
                    Reset();
                }
            }
        }
    }

    private void Attack(Vector3 dir) 
    {
        for (int i = 0; i < dataFileBullets.Length; i++)
        {
            if (dataFileBullets[i].isSleep) 
            {
                dataFileBullets[i].Initialize(dir);
                break;
            }
        }
    }

    public void Grabbing(Transform cursorTrans)
    {
        if (!isGrabed)
        {
            diffPlayerVec = ZipTrans.position - cursorTrans.position;
            pcFieldController.MovePos(this.gameObject);
        }
        ZipTrans.position = cursorTrans.position + diffPlayerVec;
        isGrabed = true;
    }

    public void Putting()
    {
        isGrabed = false;
        ZipTrans.position = pcFieldController.GetPos(this.gameObject, ZipTrans.position);
    }

    void Reset()
    {
        isSleep = true;
        renderer.enabled = false;
        pcFieldController.MovePos(this.gameObject);
        zipLifeTimer = 0;
        zipAttackTimer = 0;
        openTimer = 0;
        zipTimerText.text = "";
        pcFieldController.MovePos(this.gameObject);
        transform.position = new Vector3(0,70,0);
        isOpen = false;
    }

    public void Initialize(Vector3 pos)
    {
        ZipTrans.position = pos;
        isSleep = false;
        renderer.enabled = true;
        Putting();
    }
}
