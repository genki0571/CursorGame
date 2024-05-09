using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombEnemy : EnemyBase, IDamagable, ISelectable, IGrabbable, IHaveWeakPoint
{
    Vector3 diffPlayerVec = Vector2.zero;

    Vector3 diffWeekPointVec;

    const float ENEMY_SPEED = 4;

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
    const float BOMB_INTERVAL = 6;

    const float BOMB_RADIUS_S = 1;
    const float BOMB_STAN_INTERVAL_S = 2;

    bool stoped;

    List<BombExplotion> bombExplotions => pcFieldController.bombExplotions;


    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        diffWeekPointVec = new Vector3(Random.Range(-WEEK_POINT_POS_MAX_X, WEEK_POINT_POS_MAX_X),
            Random.Range(-WEEK_POINT_POS_MAX_Y, WEEK_POINT_POS_MAX_Y), 0);

        maxHp = BOMB_INTERVAL;

        enemySpeed = ENEMY_SPEED;
    }

    // Update is called once per frame
    void Update()
    {
        HpDisplay();

        enemyVelocity = Vector3.zero;
        Vector2 serverVec = (serverTrans.position - enemyTrans.position);
        Vector2 severDir = serverVec.normalized;

        if (state == State.None)
        {

        }
        else if (state == State.StateDecide)
        {
            if (stoped)
            {
                state = State.Stop;
            }
            else
            {
                //仮
                state = State.GoServer;
                attackTimer = 0;
            }
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
                state = State.Stop;
            }
        }
        else if (state == State.Attack)
        {

        }
        else if (state == State.Stop)
        {
            enemyVelocity = Vector3.zero;

            if (hp <= 2f)
            {
                stoped = true;
                state = State.Stop;
            }
            else 
            {
                stoped = false;
                if (serverVec.sqrMagnitude >= ATTACK_RANGE_RADIUS * ATTACK_RANGE_RADIUS)
                {
                    state = State.GoServer;
                }
            }
        }
        else if (state == State.Death)
        {
            
        }

        CheckElement();

        if (state != State.Sleep)
        {
            hp -= Time.deltaTime;

            if (hp <= 3f && state != State.Grabed)
            {
                stoped = true;
                state = State.Stop;
            }
        }

        if (hp <= 0 && state != State.Sleep)
        {
            Debug.Log("Bomb!");
            for (int i = 0; i < bombExplotions.Count; i++)
            {
                if (bombExplotions[i].isSleep)
                {
                    bombExplotions[i].Initialize(enemyTrans.position,BOMB_RADIUS_S, BOMB_STAN_INTERVAL_S);
                    break;
                }
            }

            Reset();
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
        if (state == State.Stop)
        {
            animator.SetInteger("animNum", 1);
        }
        else 
        {
            animator.SetInteger("animNum",0);
        }

        rb.velocity = enemyVelocity;
    }

    private void CheckElement() 
    {

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