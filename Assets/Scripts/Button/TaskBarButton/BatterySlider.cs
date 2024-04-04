using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerMode
{
    PowerSavingMode,
    NormalPowerMode,
    HighPowerMode,
    HighestPowerMode
}

public class BatterySlider : MonoBehaviour
{
    Transform sliderTrans;

    PCFieldController pcFieldController => PCFieldController.instance;
    PlayerInput playerInput => PlayerInput.instance;
    PlayerAttack playerAttack => PlayerAttack.instance;
    Transform playerTrans;

    Vector3 startPos;
    Vector3 endPos;
    const float SLIDER_WIDTH = 3.5f;

    bool isGrab = false;

    public float batteryNum;

    PowerMode powerMode;


    // Start is called before the first frame update
    void Start()
    {
        playerTrans = playerInput.transform;
        sliderTrans = this.transform;

        startPos = transform.position;
        endPos = startPos + new Vector3(SLIDER_WIDTH, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

        powerMode = pcFieldController.powerMode;

        if (!isGrab)
        {
            if (playerInput.IsHoldStart())
            {
                Vector3 playerPos = playerTrans.position;

                Ray2D ray = new Ray2D(playerPos, transform.up);
                RaycastHit2D hit = new RaycastHit2D();
                hit = Physics2D.Raycast(ray.origin, ray.direction, 1f, 1 << LayerMask.NameToLayer("Slider"));
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

                if (batteryNum <= 16.5)
                {
                    pcFieldController.powerMode = PowerMode.PowerSavingMode;
                    sliderTrans.position = startPos;
                    batteryNum = 0;
                }
                else if (batteryNum <= 50)
                {
                    pcFieldController.powerMode = PowerMode.NormalPowerMode;
                    sliderTrans.position = startPos + new Vector3(SLIDER_WIDTH * (1 / 3), 0, 0);
                    batteryNum = 33;
                }
                else if (batteryNum <= 83)
                {
                    pcFieldController.powerMode = PowerMode.HighPowerMode;
                    sliderTrans.position = startPos + new Vector3(SLIDER_WIDTH * (2 / 3), 0, 0);
                    batteryNum = 66;
                }
                else if (batteryNum <= 100)
                {
                    pcFieldController.powerMode = PowerMode.HighestPowerMode;
                    sliderTrans.position = endPos;
                    batteryNum = 100;
                }
            }
            else
            {
                Vector3 playerPos = playerTrans.position;
                if (playerPos.x <= startPos.x)
                {
                    batteryNum = 0;
                }
                else if (playerPos.x >= endPos.x)
                {
                    batteryNum = 100;
                }
                else
                {
                    float rate = (playerPos.x - startPos.x) / SLIDER_WIDTH;
                    batteryNum = rate * 100;
                }
            }

            sliderTrans.position = startPos + new Vector3((SLIDER_WIDTH * ((batteryNum) / 100)), 0, 0);
        }
    }
}
