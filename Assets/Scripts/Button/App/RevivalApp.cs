using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RevivalApp : MonoBehaviour
{
    PlayerState playerState => PlayerState.instance;
    Server server => Server.instance;
    Text[] texts;

    Transform appTrans;
    Vector3 appPos = new Vector3(-4.5f,3,0);

    // Start is called before the first frame update
    void Start()
    {
        texts = GetComponentsInChildren<Text>();
        appTrans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (server.isConnection && playerState.isDead) 
        {
            appTrans.position = appPos;
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].text = "";
                for (int j = 0; j < playerState.codeNums[i]; j++)
                {
                    texts[i].text += playerState.codeStrs[i][j];
                }
            }
        }
        else 
        {
            appTrans.position = new Vector3(-4.5f,100f,0);
        }
    }
}
