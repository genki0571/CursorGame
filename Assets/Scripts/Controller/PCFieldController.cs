using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCFieldController : MonoBehaviour
{
    [SerializeField] TxtTallet txtTallet;

    [SerializeField] Camera camera;
    Vector3 cameraLeftUpperPos;
    Vector3 fileLeftUpperPos;
    const float FILE_HEIGHT = 1;
    const float FILE_WIDTH = 1.5f;

    GameObject[,] putObjs = new GameObject[12, 7];
    Vector3[,] putPos = new Vector3[12,7]; 
    bool[,] isPut = new bool[12,7];

    // Start is called before the first frame update
    void Start()
    {
        //設置系のものを生成しておく
        for (int i = 0; i < 5; i++)
        {
            TxtTallet tallet = Instantiate(txtTallet, transform.position, Quaternion.identity);
            tallet.pcFieldController = this;
        }

        cameraLeftUpperPos = camera.ScreenToWorldPoint(new Vector2(0, Screen.height));
        cameraLeftUpperPos.z = 0;
        fileLeftUpperPos = cameraLeftUpperPos + new Vector3((FILE_WIDTH/2),(-FILE_HEIGHT/2),0);

        for (int i = 0; i < putPos.GetLength(0); i++)
        {
            for (int j = 0; j < putPos.GetLength(1); j++)
            {
                putPos[i,j] = fileLeftUpperPos + new Vector3(i * FILE_WIDTH,j * -FILE_HEIGHT,0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetPos(GameObject fileObj,Vector3 filePos) 
    {
        Vector3 nearPos = Vector3.zero;
        float minDistance = 10000;
        int a = 0, b = 0;

        for (int i = 0; i < putPos.GetLength(0); i++)
        {
            for (int j = 0; j < putPos.GetLength(1); j++)
            {
                if (!isPut[i,j]) 
                {
                    float distanceSqr = (putPos[i, j] - filePos).sqrMagnitude;
                    if (distanceSqr <= minDistance) 
                    {
                        minDistance = distanceSqr;
                        nearPos = putPos[i, j];
                        a = i;
                        b = j;
                    }
                }
            }
        }

        putObjs[a, b] = fileObj;
        isPut[a, b] = true;
        return nearPos;
    }

    public void MovePos(GameObject fileObj)
    {
        for (int i = 0; i < putObjs.GetLength(0); i++)
        {
            for (int j = 0; j < putObjs.GetLength(1); j++)
            {
                if (fileObj == putObjs[i,j]) 
                {
                    putObjs[i, j] = null;
                    isPut[i, j] = false;
                }
            }
        }
    }
}
