using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrugAndDrop : MonoBehaviour
{
    [SerializeField] LobbyDivision lobbyDivision;
    LobbyPosition lobbyPosition;
    // Start is called before the first frame update
    void Start()
    {
        lobbyPosition = lobbyDivision.CursorPartitioning(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("高さ:" + Screen.height);
            Debug.Log("横幅:" + Screen.width);
            Debug.Log("座標:" + transform.position);
            lobbyPosition = lobbyDivision.CursorPartitioning(transform.position);
            Debug.Log("マス目:(" + lobbyPosition.x+ "," + lobbyPosition.y+")");
        }
    }
}
