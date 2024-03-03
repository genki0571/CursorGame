using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour,IDamagable,ISelectable,IGrabbable,IHaveWeakPoint
{
    enum State 
    {
        None,
        StateDecide,
        Sleep,
        Grabed,
        GoServer,
        Attack,
        Stop
    }
    State state;

    Transform enemyTrans;

    Vector3 diffPlayerVec = Vector2.zero;

    Vector3 diffWeekPointVec;
    const float WEEK_POINT_RADIUS = 0.5f;
    const float WEEK_POINT_POS_MAX_X = 0.5f;
    const float WEEK_POINT_POS_MAX_Y = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        enemyTrans = this.GetComponent<Transform>();

        diffWeekPointVec = new Vector3(Random.Range(-WEEK_POINT_POS_MAX_X,WEEK_POINT_POS_MAX_X),
            Random.Range(-WEEK_POINT_POS_MAX_Y,WEEK_POINT_POS_MAX_Y),0);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.None)
        {

        }
        else if (state == State.StateDecide)
        {

        }
        else if (state == State.Sleep)
        {

        }
        else if (state == State.Grabed)
        {

        }
        else if (state == State.GoServer)
        {

        }
        else if(state == State.Attack) 
        {
            
        }
        else if (state == State.Stop) 
        {
            
        }
    }

    public void AddDamage(float damage) 
    {
        Debug.Log(this.name + ":" + damage);
    }

    public bool IsAttackWeekPoint(Vector3 pos)
    {
        bool isAttackWeekPoint = false;
        float sqrDistance = (pos - (enemyTrans.position + diffWeekPointVec)).sqrMagnitude;
        if (sqrDistance <= WEEK_POINT_RADIUS*WEEK_POINT_RADIUS) 
        {
            isAttackWeekPoint = true;
        }

        return isAttackWeekPoint;
    }

    public Vector3 GetWeekPoint() 
    {
        return (enemyTrans.position + diffWeekPointVec);
    }

    public void AddWeakDamage(float damage)
    {
        Debug.Log(this.name + ": 弱点 :" + damage*2);
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
