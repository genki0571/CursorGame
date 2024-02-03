using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    const float CURSOR_SPEED = 1f;

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
        cameraLeftLowPos = camera.ScreenToWorldPoint(new Vector2(0,0));
        cameraRightUpperPos = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        mouseDeltaX = Input.GetAxis("Mouse X");
        mouseDeltaY = Input.GetAxis("Mouse Y");

        Vector2 cursorPos = cursorTrans.transform.position;
        cursorPos += new Vector2(mouseDeltaX,mouseDeltaY) * CURSOR_SPEED;

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
