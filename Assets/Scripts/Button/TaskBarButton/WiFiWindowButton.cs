using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiFiWindowButton : MonoBehaviour
{
    PCFieldController pcFieldController => PCFieldController.instance;
    PlayerInput playerInput => PlayerInput.instance;
    Transform playerTrans;

    bool isAirlineMode;
    bool isInternetConnect;

    [SerializeField] GameObject airlineModeButton;
    [SerializeField] GameObject cuttingButton;

    // Start is called before the first frame update
    void Start()
    {
        playerTrans = playerInput.transform;
    }

    // Update is called once per frame
    void Update()
    {
        isAirlineMode = pcFieldController.isAirlineMode;
        isInternetConnect = pcFieldController.isInternetConnect;

        if (playerInput.IsLeftClick() || playerInput.IsHoldEnd()) 
        {
            Vector3 playerPos = playerTrans.position;

            Ray2D ray = new Ray2D(playerPos, transform.up);
            RaycastHit2D hit = new RaycastHit2D();
            hit = Physics2D.Raycast(ray.origin, ray.direction, 1f, 1 << LayerMask.NameToLayer("TaskWindowButton"));

            if (hit.collider) 
            {
                if (hit.transform.gameObject == airlineModeButton) 
                {
                    pcFieldController.isAirlineMode = !isAirlineMode;
                }

                if (hit.transform.gameObject == cuttingButton) 
                {
                    pcFieldController.isInternetConnect = !isInternetConnect;
                }
            }
        }
    }
}
