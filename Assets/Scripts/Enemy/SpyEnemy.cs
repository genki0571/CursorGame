using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpyEnemy : EnemyBase, IDamagable, ISelectable, IGrabbable, IHaveWeakPoint
{
    PCFieldController pcFieldController => PCFieldController.instance;
    Server server;
    Transform serverTrans;

    Transform enemyTrans;
    Rigidbody2D rb;

    PlayerAttack playerAttack => PlayerAttack.instance;
    Transform cursorTrans;

    Vector3 enemyVelocity;

    Vector3 diffPlayerVec = Vector2.zero;

    Vector3 diffWeekPointVec;

    const float ENEMY_SPEED = 2;

    const float ATTACK_RANGE_RADIUS = 2;
    const float ATTACK_INTERVAL = 0.5f;
    float attackTimer = 0;

    const float WEEK_POINT_RADIUS = 0.5f;
    const float WEEK_POINT_POS_MAX_X = 0.5f;
    const float WEEK_POINT_POS_MAX_Y = 0.5f;

    const float ICE_INTERVAL = 5;
    const float THUNDER_INTERVAL = 2;
    const float FIRE_INTERVAL = 8;
    const float WIND_SPEED = 20;
    const float WIND_MAX_SPEED = 60;
    const float FIRE_DURATION_DAMAGE = 5;

    const float HIDE_DIS = 5.0f;
    Vector3 elemetnPoint;

    float fireDamageTimer = 0;
    float elementTimer = 0;

    float cursorDisSqr;

    [SerializeField]bool isHide;
    bool isDiscovered;
    float hideAgainTimer = 0;
    const float HIDE_AGAIN_INTERVAL = 3;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();

        Image[] images = GetComponentsInChildren<Image>();
        for (int i = 0; i < images.Length; i++)
        {
            if (images[i].type == Image.Type.Filled)
            {
                hpImage = images[i];
                break;
            }
        }

        server = pcFieldController.server;
        serverTrans = server.transform;
        enemyTrans = this.GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        cursorTrans = playerAttack.transform;

        isHide = false;
        isDiscovered = false;
        
        diffWeekPointVec = new Vector3(Random.Range(-WEEK_POINT_POS_MAX_X, WEEK_POINT_POS_MAX_X),
            Random.Range(-WEEK_POINT_POS_MAX_Y, WEEK_POINT_POS_MAX_Y), 0);

       // Reset();
    }

    // Update is called once per frame
    void Update()
    {
        HpDisplay();

        cursorDisSqr = (cursorTrans.position - enemyTrans.position).sqrMagnitude;
        if (cursorDisSqr <= HIDE_DIS * HIDE_DIS&&isDiscovered==false)
        {
            if (state != State.Grabed)
            {
                state = State.Stop;
            }
            isHide = true;
        }
        else
        {
            isHide = false;
        }

        if (isDiscovered) 
        {
            hideAgainTimer += Time.deltaTime;
            if (hideAgainTimer >= HIDE_AGAIN_INTERVAL) 
            {
                isDiscovered = false;
                hideAgainTimer = 0;
            }
            state = State.Stop;
        }

        enemyVelocity = Vector3.zero;
        Vector2 serverVec = (serverTrans.position - enemyTrans.position);
        Vector2 severDir = serverVec.normalized;

        if (state == State.None)
        {

        }
        else if (state == State.StateDecide)
        {
            //仮
            state = State.GoServer;
            attackTimer = 0;
        }
        else if (state == State.Sleep)
        {

        }
        else if (state == State.Grabed)
        {

        }
        else if (state == State.GoServer)
        {
            enemyVelocity = severDir * ENEMY_SPEED;
            if (serverVec.sqrMagnitude <= ATTACK_RANGE_RADIUS * ATTACK_RANGE_RADIUS)
            {
                state = State.Attack;
            }
        }
        else if (state == State.Attack)
        {
            if (attackTimer <= ATTACK_INTERVAL)
            {
                attackTimer += Time.deltaTime;
            }
            if (attackTimer >= ATTACK_INTERVAL)
            {
                if (serverVec.sqrMagnitude >= ATTACK_RANGE_RADIUS * ATTACK_RANGE_RADIUS)
                {
                    Debug.Log("ATTACK");
                }
                attackTimer = 0;
                state = State.StateDecide;
            }
        }
        else if (state == State.Stop)
        {
            enemyVelocity = Vector3.zero;
            if (cursorDisSqr > HIDE_DIS * HIDE_DIS)
            {
                isHide = false;
                state = State.StateDecide;
            }
        }

        if (hp <= 0)
        {
            Reset();
        }

        //Element効果を付与されているとき
        if (haveElement != Element.Empty)
        {
            elementTimer += Time.deltaTime;

            if (haveElement == Element.Fire)
            {
                fireDamageTimer += Time.deltaTime;
                if (fireDamageTimer >= 1)
                {
                    AddDamage(FIRE_DURATION_DAMAGE);
                    fireDamageTimer = 0;
                }

                if (elementTimer >= FIRE_INTERVAL)
                {
                    fireDamageTimer = 0;
                    haveElement = Element.Empty;
                }
            }
            else if (haveElement == Element.Ice)
            {
                enemyVelocity = Vector3.zero;

                if (elementTimer >= ICE_INTERVAL)
                {
                    haveElement = Element.Empty;
                }
            }
            else if (haveElement == Element.Thunder)
            {
                enemyVelocity = Vector3.zero;

                if (elementTimer >= THUNDER_INTERVAL)
                {
                    haveElement = Element.Empty;
                }
            }
            else if (haveElement == Element.Wind)
            {
                Vector2 windVec = (enemyTrans.position - elemetnPoint);
                Vector2 windDir = windVec.normalized;
                float windDisSqr = windVec.sqrMagnitude;

                float windSpeed = WIND_SPEED * (1 / windDisSqr);
                if (windSpeed >= WIND_MAX_SPEED)
                {
                    windSpeed = WIND_MAX_SPEED;
                }
                enemyVelocity = windDir * windSpeed;


                haveElement = Element.Empty;
            }
            else if (haveElement == Element.ThunderAndWind)
            {
                Vector2 windVec = (elemetnPoint - enemyTrans.position);
                Vector2 windDir = windVec.normalized;

                float windSpeed = WIND_SPEED / 2;
                if (windSpeed >= WIND_MAX_SPEED)
                {
                    windSpeed = WIND_MAX_SPEED;
                }
                enemyVelocity = windDir * windSpeed;

                haveElement = Element.Empty;
            }

        }


        int num = 1;
        if (enemyTrans.position.x >= serverTrans.position.x)
        {
            num = -1;
        }
        else
        {
            num = 1;
        }
        enemyTrans.localScale = new Vector3(num * Mathf.Abs(enemyTrans.localScale.x), enemyTrans.localScale.y, enemyTrans.localScale.z);
        if (isHide)
        {
            animator.SetInteger("stateNum",1);
        }
        else 
        {
            if (isDiscovered)
            {
                animator.SetInteger("stateNum", 2);
            }
            else 
            {
                animator.SetInteger("stateNum",0);
            }

            if (state == State.Attack)
            {
                animator.SetInteger("animNum",1);
            }
            else if (state == State.Death)
            {
                animator.SetInteger("animNum",2);
            }
            else 
            {
                animator.SetInteger("animNum",0);
            }
        }

        rb.velocity = enemyVelocity;
    }

    public void AddDamage(float damage)
    {
        if (isHide)
        {
            damage *= 0;
        }
        DamageDisplay(enemyTrans.position + new Vector3(0, 0.5f, 0), damage);
        hp -= damage;
        Debug.Log(this.name + ":" + damage);
    }

    public void AddElement(Element element, Vector3 centerPos)
    {
        bool changeHaveElement = (haveElement != element);
        if (changeHaveElement)
        {
            elementTimer = 0;
        }
        haveElement = element;
        elemetnPoint = centerPos;
    }

    public void AddWeakDamage(float damage)
    {
        if (isHide)
        {
            damage *= 0;
        }
        DamageDisplay(enemyTrans.position + new Vector3(0, 0.5f, 0), damage * 2);
        hp -= damage * 2;
        Debug.Log(this.name + ": 弱点 :" + damage * 2);
    }


    public bool IsAttackWeekPoint(Vector3 pos)
    {
        bool isAttackWeekPoint = false;
        float sqrDistance = (pos - (enemyTrans.position + diffWeekPointVec)).sqrMagnitude;
        if (sqrDistance <= WEEK_POINT_RADIUS * WEEK_POINT_RADIUS)
        {
            isAttackWeekPoint = true;
        }

        return isAttackWeekPoint;
    }

    public Vector3 GetWeekPoint()
    {
        return (enemyTrans.position + diffWeekPointVec);
    }

    public void Selected()
    {
        //セレクト中を表示
    }

    public void Open()
    {
        if (state == State.Stop && isHide)
        {
            isDiscovered = true;
            isHide = false;
        }
        Debug.Log("Open" + ":" + this.name);
    }

    public void Delete()
    {
        DamageDisplay(enemyTrans.position + new Vector3(0, 0.5f, 0), 1000);
        hp -= 1000;
        Debug.Log("Delete");
    }

    public void Grabbing(Transform cursorTrans)
    {
        if (state != State.Grabed)
        {
            diffPlayerVec = enemyTrans.position - cursorTrans.position;
        }
        enemyTrans.position = cursorTrans.position + diffPlayerVec;
        state = State.Grabed;
    }

    public void Putting()
    {
        state = State.StateDecide;
    }

}