using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Server : MonoBehaviour,IGrabbable
{
    Transform serverTrans;
    bool isGrabed;

    Vector3 diffPlayerVec = Vector2.zero;
    [SerializeField] PCFieldController pcFieldController;

    float serverHp = 0;

    // Start is called before the first frame update
    void Start()
    {
        serverTrans = GetComponent<Transform>();
        Putting();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Grabbing(Transform cursorTrans)
    {
        if (!isGrabed)
        {
            diffPlayerVec = serverTrans.position - cursorTrans.position;
            pcFieldController.MovePos(this.gameObject);
        }
        serverTrans.position = cursorTrans.position + diffPlayerVec;
        isGrabed = true;
    }

    public void Putting()
    {
        isGrabed = false;

        serverTrans.position = pcFieldController.GetPos(this.gameObject, serverTrans.position);

    }

    public void AddDamage(float damage) 
    {
        serverHp -= damage;
    }
}
