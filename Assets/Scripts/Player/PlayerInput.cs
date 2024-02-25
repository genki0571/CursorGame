using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Player player;


    const int LONGPRESS_INTERVAL_FRAME = 40;
    int longPressCnt;

    [SerializeField] bool isLeftClick;
    [SerializeField] bool isRightClick;
    [SerializeField] bool isLongPressDown;
    [SerializeField] bool isLongPress;

    private void Start()
    {
        player = GetComponent<Player>();
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
            longPressCnt = 0;
            isLeftClick = false;
            isLongPress = false;
            isLongPressDown = true;
        }
        else if (Input.GetMouseButton(0))
        {
            longPressCnt += 1;
            isLeftClick = false;
            isLongPress = false;
            isLongPressDown = false;

            //長押ししているなら
            if (longPressCnt >= LONGPRESS_INTERVAL_FRAME) 
            {
                isLongPress = true;
                //Debug.Log("LongPress");
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (longPressCnt <= LONGPRESS_INTERVAL_FRAME)
            {
                isLeftClick = true;
                //Debug.Log("LeftClick");
            }
            isLongPress = false;
            isLongPressDown = false;
        }
        else 
        {
            isLeftClick = false;
            isLongPress = false;
            isLongPressDown = false;
        }

        //Playerに代入
        player.isLeftClick = isLeftClick;
        player.isRightClick = isRightClick;
        player.isLongPressDown = isLongPressDown;
        player.isLongPress = isLongPress;

    }
}
