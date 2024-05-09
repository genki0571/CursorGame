using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallEnemy : EnemyBase, IDamagable, ISelectable, IGrabbable, IHaveWeakPoint
{
    float damage = 5;
    PlayerAttack playerAttack => PlayerAttack.instance;
    Transform cursorTrans;

    Vector3 enemyVelocity;

    Vector3 diffPlayerVec = Vector2.zero;

    Vector3 diffWeekPointVec;

    const float ENEMY_SPEED = 4;

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

    Vector3 elemetnPoint;

    float fireDamageTimer = 0;
    float elementTimer = 0;

    float cursorDisSqr;

    bool isRunAway;
    bool isRunUp;
    bool isRunRight;
    bool isRunDir;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        isRunAway = false;
        isRunUp = true;
        isRunRight = true;
        isRunDir = true;
        cursorTrans = playerAttack.transform;
        /*diffWeekPointVec = new Vector3(Random.Range(-WEEK_POINT_POS_MAX_X, WEEK_POINT_POS_MAX_X),
            Random.Range(-WEEK_POINT_POS_MAX_Y, WEEK_POINT_POS_MAX_Y), 0);

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
    
        
        Reset();
        */
    }

    // Update is called once per frame
    void Update()
    {
        HpDisplay();

        cursorDisSqr = (cursorTrans.position - enemyTrans.position).sqrMagnitude;
        enemyVelocity = Vector3.zero;
        Vector2 serverVec = (serverTrans.position - enemyTrans.position);
        Vector2 severDir = serverVec.normalized;
        Vector2 cursorVec = (cursorTrans.position - enemyTrans.position);
        Vector2 cursorDir = cursorVec.normalized;
        Vector3 tangentVec;
        if (cursorTrans.position.y > enemyTrans.position.y)// || (enemyTrans.position.y > ATTACK_RANGE_RADIUS * 0.9f && cursorDisSqr < 0.5f))
        {
            isRunUp = false;
        }
        else if(cursorTrans.position.y <= enemyTrans.position.y)// || (enemyTrans.position.y > -ATTACK_RANGE_RADIUS * 0.9f && cursorDisSqr < 0.5f))
        {
            isRunUp = true;
        }

        if(cursorTrans.position.x > enemyTrans.position.x)
        {
            isRunRight = false;
        }
        else if(cursorTrans.position.x <= enemyTrans.position.x)
        {
            isRunRight = true;
        }

        if(enemyTrans.position.y >= ATTACK_RANGE_RADIUS * 0.9 || enemyTrans.position.y <= -ATTACK_RANGE_RADIUS * 0.9)
        {
            isRunDir = false;
        }
        else if (enemyTrans.position.x >= ATTACK_RANGE_RADIUS * 0.9 || enemyTrans.position.x <= -ATTACK_RANGE_RADIUS * 0.9)
        {
            isRunDir = true;
        }
        if (isRunDir)
        {
            if (isRunUp)
            {
                tangentVec = Vector3.Cross(serverVec.normalized, Vector3.up).normalized;
                tangentVec = Vector3.Cross(tangentVec, serverVec.normalized);
                Debug.Log("up");
            }
            else
            {
                tangentVec = Vector3.Cross(serverVec.normalized, Vector3.down).normalized;
                tangentVec = Vector3.Cross(tangentVec, serverVec.normalized);
                Debug.Log("down");
            }
        }
        else
        {
            if (isRunRight)
            {
                tangentVec = Vector3.Cross(serverVec.normalized, Vector3.right).normalized;
                tangentVec = Vector3.Cross(tangentVec, serverVec.normalized);
                Debug.Log("right");
            }
            else
            {
                tangentVec = Vector3.Cross(serverVec.normalized, Vector3.left).normalized;
                tangentVec = Vector3.Cross(tangentVec, serverVec.normalized);
                Debug.Log("left");
            }
        }
        if (cursorDisSqr <= 1.0f)
        {
            isRunAway = true;
        }
        else
        {
            isRunAway = false;
        }

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
            if (isRunAway)
            {
                enemyVelocity = tangentVec * ENEMY_SPEED * 1.5f;
            }
            else
            {
                enemyVelocity = severDir * ENEMY_SPEED;
            }
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
                    server.AddDamage(damage);
                }
                attackTimer = 0;
                state = State.StateDecide;
            }
            if (isRunAway)
            {
                state = State.StateDecide;
            }
        }
        else if (state == State.Stop)
        {
            
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

        rb.velocity = enemyVelocity;
    }

    public void AddDamage(float damage)
    {
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