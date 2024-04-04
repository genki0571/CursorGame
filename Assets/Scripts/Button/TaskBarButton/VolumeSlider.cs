using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeSlider : MonoBehaviour
{
    Transform sliderTrans;
    PCFieldController pcFieldController => PCFieldController.instance;

    PlayerInput playerInput => PlayerInput.instance;
    PlayerAttack playerAttack => PlayerAttack.instance;
    Transform playerTrans;

    Vector3 startPos;
    Vector3 endPos;
    const float SLIDER_WIDTH = 4;

    bool isGrab = false;

    float volumeNum;


    // Start is called before the first frame update
    void Start()
    {

        playerTrans = playerInput.transform;
        sliderTrans = this.transform;

        startPos = transform.position;
        endPos = startPos + new Vector3(SLIDER_WIDTH,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        volumeNum = pcFieldController.volumeNum;

        if (!isGrab) 
        {
            if (playerInput.IsHoldStart()) 
            {
                Vector3 playerPos = playerTrans.position;

                Ray2D ray = new Ray2D(playerPos, transform.up);
                RaycastHit2D hit = new RaycastHit2D();
                hit = Physics2D.Raycast(ray.origin, ray.direction, 1f,1 << LayerMask.NameToLayer("Slider"));
                if (hit.collider)
                {
                    if (hit.transform.gameObject == this.gameObject)
                    {
                        isGrab = true;
                        playerAttack.isGrabSlider = true;
                    }
                }
            }
        }
        else 
        {
            if (playerInput.IsHoldEnd())
            {
                isGrab = false;
                playerAttack.isGrabSlider = false;
            }
            else
            {
                Vector3 playerPos = playerTrans.position;
                if (playerPos.x <= startPos.x)
                {
                    volumeNum = 0;
                }
                else if (playerPos.x >= endPos.x)
                {
                    volumeNum = 100;
                }
                else
                {
                    float rate = (playerPos.x - startPos.x) / SLIDER_WIDTH;
                    volumeNum = rate * 100;
                }
            }

            pcFieldController.volumeNum = volumeNum;
        }

        sliderTrans.position = startPos + new Vector3((SLIDER_WIDTH * ((volumeNum) / 100)) , 0, 0);
    }
}
