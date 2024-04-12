using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerState playerState => PlayerState.instance;

    const float CURSOR_SPEED = 0.5f;

    //カーソル位置
    Transform cursorTrans;
    float mouseDeltaX;
    float mouseDeltaY;

    //カメラ位置
    [SerializeField] Camera camera;
    Vector2 cameraLeftLowPos;
    Vector2 cameraRightUpperPos;

    // Start is called before the first frame update
    void Start()
    {
        cursorTrans = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float speed = CURSOR_SPEED;
        if (playerState.isStan)
        {
            speed = 0;
        }
        else if (playerState.IsVirus(Virus.SpeedUp)) 
        {
            speed *= 8;   
        }
        else if (playerState.IsVirus(Virus.SpeedDown))
        {
            speed /= 10;
        }

        if (playerState.IsVirus(Virus.MoveInvert)) 
        {
            speed *= -1;
        }

        if (playerState.isDead) 
        {
            speed = CURSOR_SPEED / 30;
        }

        cameraLeftLowPos = camera.ScreenToWorldPoint(new Vector2(0,0));
        cameraRightUpperPos = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        mouseDeltaX = Input.GetAxis("Mouse X");
        mouseDeltaY = Input.GetAxis("Mouse Y");

        Vector2 cursorPos = cursorTrans.transform.position;
        cursorPos += new Vector2(mouseDeltaX,mouseDeltaY) * speed;

        if (cursorPos.x <= cameraLeftLowPos.x) 
        {
            cursorPos.x = cameraLeftLowPos.x;
        }
        if (cursorPos.x >= cameraRightUpperPos.x)
        {
            cursorPos.x = cameraRightUpperPos.x;
        }
        if (cursorPos.y <= cameraLeftLowPos.y)
        {
            cursorPos.y = cameraLeftLowPos.y;
        }
        if (cursorPos.y >= cameraRightUpperPos.y)
        {
            cursorPos.y = cameraRightUpperPos.y;
        }

        
        cursorTrans.position = cursorPos;
    }

    
}
