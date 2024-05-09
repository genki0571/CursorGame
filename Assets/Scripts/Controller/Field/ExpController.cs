using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpController : MonoBehaviour
{
    static public ExpController instance;

    ServerAttack serverAttack => ServerAttack.instance;
    Transform serverTrans;

    [SerializeField]GameObject expS;
    [SerializeField]GameObject expM;
    float expSSizeX;
    float expMSizeX;


    PlayerState playerState => PlayerState.instance;
    Transform playerTrans;
    Vector3 playerPos = Vector3.zero;
    Vector3 playerPosBefore = Vector3.zero;
    [SerializeField]int playerExpNum = 0;
    int playerExpNumMax = 20;
    [SerializeField] List<Transform> playerExpTransforms = new List<Transform>();

    public List<Transform> expSTransforms = new List<Transform>();
    public List<Transform> expMTransforms = new List<Transform>();

    public List<Transform> activeExpSTransforms = new List<Transform>();
    public List<Transform> activeExpMTransforms = new List<Transform>();

    const float GET_RADIUS_SQR = 0.64f;

    float timer;
    float interval = 0.01f;

    const float SERVER_GET_RADIUS_SQR = 1f;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        expSSizeX = expS.transform.localScale.x;
        expMSizeX = expM.transform.localScale.x;

        Transform[] exps = GetComponentsInChildren<Transform>();
        for (int i = 0; i < exps.Length; i++)
        {
            if (exps[i].localScale.x == expSSizeX)
            {
                expSTransforms.Add(exps[i]);
            }
            else if(exps[i].localScale.x == expMSizeX) 
            {
                expMTransforms.Add(exps[i]);  
            }
        }

        playerTrans = playerState.transform;
        serverTrans = serverAttack.transform;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = playerTrans.position;
        for (int i = 0; i < activeExpSTransforms.Count; i++)
        {
            float expLengthSqr = (playerPos - activeExpSTransforms[i].position).sqrMagnitude;
            if (expLengthSqr <= GET_RADIUS_SQR) 
            {
                GetExp(activeExpSTransforms[i]);
            }
        }
        for (int i = 0; i < activeExpMTransforms.Count; i++)
        {
            float expLengthSqr = (playerPos - activeExpMTransforms[i].position).sqrMagnitude;
            if (expLengthSqr <= GET_RADIUS_SQR)
            {
                GetExp(activeExpMTransforms[i]);
            }
        }

        if (timer <= interval)
        {
            timer += Time.deltaTime;
        }
        if (timer >= interval && playerExpTransforms.Count != 0)
        {
            FollowPlayerExp();
        }
        playerPosBefore = playerPos;

        float serverDistanceSqr = (playerPos - serverTrans.position).sqrMagnitude;
        if (serverDistanceSqr <= SERVER_GET_RADIUS_SQR)
        {
            AddServerExp();
        }
    }

    public void InitializeExpS(Vector3 pos)
    {
        bool isExsist = false;
        Transform activeExp = null;
        for (int i = 0; i < expSTransforms.Count; i++)
        {
            bool isSleep = true;
            for (int j = 0; j < activeExpSTransforms.Count; j++)
            {
                if (activeExpSTransforms[j] == expSTransforms[i])
                {
                    isSleep = false;
                    break;
                }
            }

            if (isSleep)
            {
                activeExpSTransforms.Add(expSTransforms[i]);
                activeExp = expSTransforms[i];
                isExsist = true;
                break;
            }
        }

        if (!isExsist) 
        {
            Transform exp = Instantiate(expS,transform.position,Quaternion.identity).transform;
            exp.parent = this.transform;
            activeExp = exp;
            expSTransforms.Add(exp);
            activeExpMTransforms.Add(exp);
        }

        activeExp.position = pos;
    }

    public void InitializeExpM(Vector3 pos)
    {
        bool isExsist = false;
        Transform activeExp = null;
        for (int i = 0; i < expMTransforms.Count; i++)
        {
            bool isSleep = true;
            for (int j = 0; j < activeExpMTransforms.Count; j++)
            {
                if (activeExpMTransforms[j] == expMTransforms[i])
                {
                    isSleep = false;
                    break;
                }
            }

            if (isSleep)
            {
                activeExpMTransforms.Add(expMTransforms[i]);
                activeExp = expMTransforms[i];
                isExsist = true;
                break;
            }
        }

        if (!isExsist) 
        {
            Transform exp = Instantiate(expM,transform.position,Quaternion.identity).transform;
            exp.parent = this.transform;
            expMTransforms.Add(exp);
            activeExpMTransforms.Add(exp);
        }

        activeExp.position = pos;
    }

    void GetExp(Transform expTrans) 
    {
        if (playerExpNum < playerExpNumMax) 
        {
            playerExpTransforms.Add(expTrans);
            playerExpNum = playerExpTransforms.Count;
            if (expTrans.localScale.x == expSSizeX) 
            {
                activeExpSTransforms.Remove(expTrans);
            }
            else if(expTrans.localScale.x == expMSizeX)
            {
                activeExpMTransforms.Remove(expTrans);
            }
            ResetExp(expTrans);
        }
    }

    private void ResetExp(Transform expTrans)
    {

    }

    void FollowPlayerExp() 
    {
        for (int i = playerExpNum - 1; i >= 0; i--)
        {
            if (i == 0 && playerExpTransforms.Count != 0)
            {
                playerExpTransforms[i].position = playerPosBefore;
            }
            else 
            {
                playerExpTransforms[i].position += (playerExpTransforms[i - 1].position - playerExpTransforms[i].position) / 6; //数を大きくするほど遅くなる
            }
        }
    }

    void AddServerExp() 
    {
        serverAttack.GetExp(playerExpNum);
        for (int i = 0; i < playerExpNum; i++)
        {
            playerExpTransforms[i].localPosition = Vector3.zero;
        }
        playerExpTransforms.Clear();
        playerExpNum = 0;
    }
}
