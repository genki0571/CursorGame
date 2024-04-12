using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplotion : MonoBehaviour
{
    PlayerState playerState => PlayerState.instance;
    Transform playerTrans;
    Transform explotionTrans;

    const float RADIUS_S = 2;
    const float RADIUS_M = 3;
    const float RADIUS_L = 4;

    const float INTERVAL_S = 3;
    const float INTERVAL_M = 2;
    const float INTERVAL_L = 1;

    public bool isSleep;

    // Start is called before the first frame update
    void Start()
    {
        playerTrans = playerState.transform;
        explotionTrans = this.transform;

        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Reset()
    {
        isSleep = true;
    }

    public void Initialize()
    {
        isSleep = false;
        Explotion();
    }

    private void Explotion() 
    {
        float sqrDistance = (playerTrans.position - explotionTrans.position).sqrMagnitude;
        if (sqrDistance <= RADIUS_S * RADIUS_S)
        {
            playerState.GetStan(INTERVAL_S);
        }
        else if (sqrDistance <= RADIUS_M * RADIUS_M)
        {
            playerState.GetStan(INTERVAL_M);
        }
        else if (sqrDistance <= RADIUS_L * RADIUS_L)
        {
            playerState.GetStan(INTERVAL_L);
        }
        Reset();
    }
}
