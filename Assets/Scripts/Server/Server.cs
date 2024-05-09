using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Server : MonoBehaviour,IGrabbable
{
    static public Server instance;

    Transform serverTrans;
    bool isGrabed;

    Vector3 diffPlayerVec = Vector2.zero;
    PCFieldController pcFieldController => PCFieldController.instance;
    PlayerState playerState => PlayerState.instance;
    Transform playerTrans;

    public float maxServerHp = 100;
    public float serverHp;

    [SerializeField] float recoveryRadius;
    int recoveryAmount = 1;
    float recoveryTimer = 0;
    const float RECOVERY_INTERVAL = 4f;

    public bool isConnection;

    void Awake() 
    {
        instance = this;
        serverHp = maxServerHp;
    }

    // Start is called before the first frame update
    void Start()
    {
        serverTrans = GetComponent<Transform>();
        playerTrans = playerState.transform;
        Putting();
    }

    // Update is called once per frame
    void Update()
    {
        float sqrDis = (playerTrans.position - serverTrans.position).sqrMagnitude;
        if (sqrDis <= recoveryRadius * recoveryRadius) 
        {
            isConnection = true;
        }
        else 
        {
            recoveryTimer = 0;
            isConnection = false;
        }

        if (isConnection) 
        {
            if (!playerState.isDead) 
            {
                recoveryTimer += Time.deltaTime;
                if (recoveryTimer >= RECOVERY_INTERVAL)
                {
                    playerState.Recovery(recoveryAmount);
                    recoveryTimer = 0;
                }
            }
        }
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
        Debug.Log("Server:"+damage);
    }
}
