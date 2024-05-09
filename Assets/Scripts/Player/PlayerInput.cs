using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput instance;

    const int HOLD_INTERVAL_FRAME = 30;
    int holdCnt;

    [SerializeField] bool isLeftClick;
    [SerializeField] bool isRightClick;
    [SerializeField] bool isHoldStart;
    [SerializeField] bool isHold;
    [SerializeField] bool isHoldEnd;

    const float HOLD_JUDGE_RUDIUS_SQR = 0.09f;
    Transform playerTrans;
    Vector3 leftClickPos;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerTrans = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //右クリックの判定
        if (Input.GetMouseButtonDown(1))
        {
            isRightClick = true;
            //Debug.Log("RightClick");
        }
        else 
        {
            isRightClick = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            holdCnt = 0;
            isLeftClick = false;
            isHold = false;
            isHoldStart = true;
            isHoldEnd = false;
            leftClickPos = playerTrans.position;
        }
        else if (Input.GetMouseButton(0))
        {
            holdCnt += 1;
            isLeftClick = false;
            isHold = false;
            isHoldStart = false;
            isHoldEnd = false;

            //長押ししているなら
            if (holdCnt >= HOLD_INTERVAL_FRAME) 
            {
                isHold = true;
                //Debug.Log("Hold");
            }
            //左クリックから一定以上移動したら
            float disSqr = (playerTrans.position - leftClickPos).sqrMagnitude;
            if (disSqr >= HOLD_JUDGE_RUDIUS_SQR) 
            {
                isHold = true;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (holdCnt <= HOLD_INTERVAL_FRAME)
            {
                isLeftClick = true;
                //Debug.Log("LeftClick");
            }
            isHold = false;
            isHoldStart = false;
            isHoldEnd = true;
            
        }
        else 
        {
            isLeftClick = false;
            isHold = false;
            isHoldStart = false;
            isHoldEnd = false;
        }
    }

    public bool IsLeftClick() 
    {
        return isLeftClick;
    }

    public bool IsRightClick()
    {
        return isRightClick;
    }
    public bool IsHold()
    {
        return isHold;
    }
    public bool IsHoldStart()
    {
        return isHoldStart;
    }
    public bool IsHoldEnd()
    {
        return isHoldEnd;
    }
}
