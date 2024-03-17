using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallText : MonoBehaviour
{
    float attackDamage = 30;

    float fallSpeed;
    const float START_FALL_POS_Y = 5;

    const float ATTACK_INTERVAL = 0.7f;
    float attackTimer = 0;

    public bool isSleep;

    List<GameObject> withinDamagables;

    RangeCheck rangeCheck;
    Transform trans;
    GameObject canvas;
    SpriteRenderer renderer;

    [SerializeField]private AnimationCurve linear;

    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>();
        rangeCheck = GetComponent<RangeCheck>();
        withinDamagables = rangeCheck.withinDamegableObjects;
        canvas = GetComponentInChildren<Canvas>().gameObject;
        renderer = GetComponent<SpriteRenderer>();

        linear = AnimationCurve.Linear(
        timeStart: 0f,
        valueStart: START_FALL_POS_Y,
        timeEnd: ATTACK_INTERVAL,
        valueEnd: 0f
        );

        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSleep)
        {
            if (attackTimer <= ATTACK_INTERVAL)
            {
                attackTimer += Time.deltaTime;
            }

            canvas.transform.localPosition = new Vector3(0,linear.Evaluate(attackTimer),0);

            if (attackTimer >= ATTACK_INTERVAL)
            {
                Attack();
                Reset();
            }
        }
    }

    private void Attack() 
    {
        for (int i = 0; i < withinDamagables.Count; i++)
        {
            IDamagable damagable = withinDamagables[i].GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.AddDamage(attackDamage);
            }
        }
    }

    public void Reset()
    {
        isSleep = true;
        trans.localPosition = Vector3.zero;
        attackTimer = 0;
        canvas.SetActive(false);
        renderer.enabled = false;
    }

    public void Initialize(Vector3 pos)
    {
        isSleep = false;
        trans.position = pos;
        canvas.SetActive(true);
        renderer.enabled = true;
    }
}
