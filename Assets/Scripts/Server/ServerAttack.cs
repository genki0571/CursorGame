using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerAttack : MonoBehaviour
{
    static public ServerAttack instance;

    PlayerScroll playerScroll => PlayerScroll.instace;
    float scrollAmount = 0;
    [SerializeField] Transform serverAttackDirTrans;

    int level = 0;


    private void Awake()
    {
        instance = this;
        Debug.Log(playerScroll);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scrollAmount = playerScroll.scrollAmount;

        serverAttackDirTrans.localEulerAngles = new Vector3(0,0,scrollAmount);
    }

    public void GetExp(int expNum) 
    {
        //Debug.Log(expNum + "の経験値");
    }

}
