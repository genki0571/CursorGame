using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeekPoint : MonoBehaviour
{
    public bool isSleep;

    float timer = 0;
    float showInterval = 0;

    IHaveWeakPoint inChargeEnemy;
    Transform pointTrans;

    // Start is called before the first frame update
    void Start()
    {
        pointTrans = this.transform;
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSleep)
        {
            timer += Time.deltaTime;
            if (timer >= showInterval)
            {
                Reset();
            }
            else 
            {
                pointTrans.position = inChargeEnemy.GetWeekPoint();
            }
        }
    }

    public void ShowPoint(IHaveWeakPoint enemy,float interval)
    {
        isSleep = false;
        inChargeEnemy = enemy;
        showInterval = interval;
    }

    private void Reset()
    {
        isSleep = true;
        timer = 0;

        transform.position = new Vector3(0,100,0);
    }
}
