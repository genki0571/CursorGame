using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour,IDamagable,ISelectable,IGrabbable
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

    // Start is called before the first frame update
    void Start()
    {
        enemyTrans = this.GetComponent<Transform>();
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

    public void Selected()
    {
        
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
