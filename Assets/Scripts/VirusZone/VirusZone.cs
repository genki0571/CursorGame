using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusZone : MonoBehaviour
{
    PlayerState playerState => PlayerState.instance;
    Transform playerTrans;

    Transform virusTrans;

    [SerializeField] Virus virus;
    [SerializeField] float rangeRadius;

    bool isInfection;

    // Start is called before the first frame update
    void Start()
    {
        playerTrans = playerState.transform;
        virusTrans = this.transform;
        virusTrans.localScale = new Vector3(rangeRadius*2,rangeRadius*2,0);
    }

    // Update is called once per frame
    void Update()
    {
        float sqrDistance = (playerTrans.position - virusTrans.position).sqrMagnitude;
        if (sqrDistance <= rangeRadius * rangeRadius) 
        {
            if (!isInfection && virus != Virus.Empty)
            {
                playerState.GetVirus(virus);
            }
            isInfection = true;
        }
        else 
        {
            if (isInfection) 
            {
                playerState.RemoveVirus(virus);
            }
            isInfection = false;
        }
    }
}
