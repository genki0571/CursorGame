﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeButton : MonoBehaviour,IButton
{
    GameObject volumeWindow;

    PlayerInput playerInput => PlayerInput.instance;
    Transform playerTrans;

    [SerializeField]bool isOpen;
    [SerializeField]bool isClick;

    SpriteRenderer renderer;
    [SerializeField] Color32 originalColor;
    [SerializeField] Color32 selectedColor;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        originalColor = renderer.color;

        volumeWindow = this.transform.GetChild(0).gameObject;
        playerTrans = playerInput.transform;
        volumeWindow.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        Color32 color = new Color32(255, 255, 255, 255);

        Vector3 playerPos = playerTrans.position;

        Ray2D ray = new Ray2D(playerPos, transform.forward);
        RaycastHit2D hit = new RaycastHit2D();
        hit = Physics2D.Raycast(ray.origin, ray.direction, 1f, 1 << LayerMask.NameToLayer("TaskWindowButton"));
        if (hit.collider)
        {
            if (hit.transform == this.transform)
            {
                color = selectedColor;
            }
            else
            {
                color = originalColor;
            }
        }
        else
        {
            color = originalColor;
        }

        if (renderer.color != color)
        {
            renderer.color = color;
        }

        if (isClick) 
        {
            if (!playerInput.IsLeftClick() && !playerInput.IsRightClick() && !playerInput.IsHoldStart())
            {
                isClick = false;
                isOpen = true;
            }
        }

        if (isOpen && !isClick)
        {
            volumeWindow.transform.gameObject.SetActive(true);

            if (playerInput.IsLeftClick() || playerInput.IsRightClick() || playerInput.IsHoldStart()) 
            {
                if (hit.collider)
                {
                    Debug.Log(hit.transform.name);
                    if (hit.transform.gameObject != volumeWindow)
                    {
                        isOpen = false;
                        Close();
                    }
                }
                else 
                {
                    isOpen = false;
                    Close();
                }
            }
        }
    }

    private void Close() 
    {
        volumeWindow.transform.gameObject.SetActive(false);
    }

    public Command Pushed()
    {
        isClick = true;

        Command command = Command.Empty;
        return command;
    }
}
