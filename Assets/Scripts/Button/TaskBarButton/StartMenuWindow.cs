using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuWindow : MonoBehaviour
{
    PCFieldController pcFieldController => PCFieldController.instance;
    PlayerInput playerInput => PlayerInput.instance;
    Transform playerTrans;

    public bool isShowButton;

    [SerializeField] GameObject powerSupplyButton;
    [SerializeField] GameObject shutdownButton;

    // Start is called before the first frame update
    void Start()
    {
        playerTrans = playerInput.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.IsLeftClick() || playerInput.IsHoldEnd())
        {
            Vector3 playerPos = playerTrans.position;

            Ray2D ray = new Ray2D(playerPos, transform.up);
            RaycastHit2D hit = new RaycastHit2D();
            hit = Physics2D.Raycast(ray.origin, ray.direction, 1f, 1 << LayerMask.NameToLayer("TaskWindowButton"));

            if (hit.collider)
            {
                if (hit.transform.gameObject == powerSupplyButton)
                {
                    isShowButton = true;
                }
                else if (hit.transform.gameObject == shutdownButton) 
                {
                    Debug.Log("Shutdown");
                }
                else
                {
                    isShowButton = false;
                }
            }
            else 
            {
                isShowButton = false;
            }
        }


        if (isShowButton) 
        {
            shutdownButton.SetActive(true);
        }
        else
        {
            shutdownButton.SetActive(false);
        }
    }
}
