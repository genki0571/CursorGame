using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEnemy : EnemyBase, IDamagable, ISelectable, IGrabbable, IHaveWeakPoint
{
    PCFieldController pcFieldController => PCFieldController.instance;
    Server server;
    Transform serverTrans;

    Transform enemyTrans;
    Rigidbody2D rb;

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
    float bombTimer = 0;

    int count = 3;
    // Start is called before the first frame update
    void Start()
    {
        server = pcFieldController.server;
        serverTrans = server.transform;
        enemyTrans = this.GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();

        diffWeekPointVec = new Vector3(Random.Range(-WEEK_POINT_POS_MAX_X, WEEK_POINT_POS_MAX_X),
            Random.Range(-WEEK_POINT_POS_MAX_Y, WEEK_POINT_POS_MAX_Y), 0);
    }

    // Update is called once per frame
    void Update()
    {
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

        }
        else if (state == State.Death)
        {
            bombTimer += Time.deltaTime;
            if (bombTimer >= 3)
            {
                Debug.Log("Bomb!");
                //Instantiate(爆発,enemyTrans,Quaternion.identity);
                Delete();
            }
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
        Debug.Log("Delete");
        Destroy(this.gameObject);
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