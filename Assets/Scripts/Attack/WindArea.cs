using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArea : MonoBehaviour
{
    Transform areaTrans;
    Transform rangeCheckTrans;

    SpriteRenderer renderer;

    RangeCheck rangeCheck;
    List<GameObject> withinObj;

    float timer = 0;
    const float INTERVAL = 5;

    public bool isSleep;

    // Start is called before the first frame update
    void Start()
    {
        areaTrans = GetComponent<Transform>();
        renderer = GetComponent<SpriteRenderer>();
        rangeCheck = GetComponentInChildren<RangeCheck>();
        rangeCheckTrans = rangeCheck.GetComponent<Transform>();
        withinObj = rangeCheck.withinDamegableObjects;
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSleep) 
        {
            if (timer <= INTERVAL)
            {
                timer += Time.deltaTime;
            }
            if (timer >= INTERVAL) 
            {
                Reset();
            }

            for (int i = 0; i < rangeCheck.withinDamegableObjects.Count; i++)
            {
                IDamagable damagable = withinObj[i].GetComponent<IDamagable>();
                if (damagable != null) 
                {
                    damagable.AddElement(Element.Wind,areaTrans.position);
                }
            }
        }
    }

    public void Reset()
    {
        isSleep = true;
        renderer.enabled = false;
        timer = 0;
    }

    public void Initialize(Transform pressAreaTrans)
    {
        isSleep = false;
        renderer.enabled = true;
        areaTrans.localScale = pressAreaTrans.localScale;
        areaTrans.position = pressAreaTrans.position;
        rangeCheckTrans.localPosition = Vector3.zero;
        rangeCheckTrans.localRotation = Quaternion.identity;
    }
}
