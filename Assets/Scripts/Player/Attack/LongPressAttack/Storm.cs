using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storm : MonoBehaviour
{
    const float INTERVAL = 5;
    float timer = 0;
    const float STORM_SPEED = 2;
    const float BORDER_LENGTH = 3;

    public bool isSleep;

    Transform stormTrans;
    SpriteRenderer renderer;
    Rigidbody2D rb;

    Vector3 centerPoint = Vector2.zero;
    Vector3 popPoint;
    float popLength;
    [SerializeField] AnimationCurve curve1;
    [SerializeField] AnimationCurve curve2;
    Quaternion quaternion = Quaternion.Euler(0,0,0);

    RangeCheck rangeCheck;
    Transform rangeCheckTrans;
    List<GameObject> withinEnemies;

    // Start is called before the first frame update
    void Start()
    {
        stormTrans = GetComponent<Transform>();
        renderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rangeCheck = GetComponentInChildren<RangeCheck>();
        rangeCheckTrans = rangeCheck.GetComponent<Transform>();
        withinEnemies = rangeCheck.withinDamegableObjects;
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSleep) 
        {
            rangeCheckTrans.localPosition = Vector3.zero;

            Vector3 centerToStormVec = stormTrans.position - centerPoint;
            Vector3 centerToStormDir = centerToStormVec.normalized;
            float length = 0;
            if (popLength <= BORDER_LENGTH)
            {
                length = curve1.Evaluate(timer) + popLength;
            }
            else
            {
                length = curve2.Evaluate(timer) + popLength;
            }
            Vector3 centerToMovePosDir = quaternion * centerToStormDir;
            Vector3 centerToMovePosVec = centerToMovePosDir * length;
            Vector3 movePos = centerPoint + centerToMovePosVec;
            Vector3 moveDir = (movePos - stormTrans.position).normalized;

            rb.velocity = moveDir * STORM_SPEED;

            for (int i = 0; i < withinEnemies.Count; i++)
            {
                IDamagable damagable = withinEnemies[i].GetComponent<IDamagable>();
                if (damagable != null)
                {
                    damagable.AddElement(Element.ThunderAndWind,stormTrans.position);
                }
            }

            //時間を計測
            if (timer <= INTERVAL)
            {
                timer += Time.deltaTime;
            }
            if (timer >= INTERVAL)
            {
                Reset();
            }
        }
        
    }

    public void Reset()
    {
        isSleep = true;
        renderer.enabled = false;
        timer = 0;
        rb.velocity = Vector2.zero;
    }

    public void Initialize(Vector3 pos)
    {
        stormTrans.position = pos;
        popPoint = pos;
        popLength = (popPoint - centerPoint).magnitude;
        int rand = Random.Range(0,2);
        if (rand == 0)
        {
            quaternion = Quaternion.Euler(0, 0, 8);
        }
        else
        {
            quaternion = Quaternion.Euler(0, 0, -8);
        }

        isSleep = false;
        renderer.enabled = true;
    }
}
