using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCFieldController : MonoBehaviour
{
    public static PCFieldController instance;

    //Prefabs
    [SerializeField] TxtTallet txtTallet;
    [SerializeField] PngBuster pngBuster;
    [SerializeField] Hammer hammerAttack;
    [SerializeField] LoadTunder loadTunderAttack;
    [SerializeField] FireWallPost fireWallPost;
    [SerializeField] FireWall fireWall;
    [SerializeField] public GameObject scanTargetObj;
    
    //Pool
    public List<TxtTallet> txtTallets = new List<TxtTallet>();
    public List<PngBuster> pngBusters = new List<PngBuster>();
    public List<Hammer> hammers = new List<Hammer>();
    public List<LoadTunder> loadTunders = new List<LoadTunder>();
    public List<FireWallPost> fireWallPosts = new List<FireWallPost>();
    public List<FireWall> fireWalls = new List<FireWall>();

    [SerializeField] Camera camera;
    Vector3 cameraLeftUpperPos;
    Vector3 fileLeftUpperPos;
    const float FILE_HEIGHT = 1;
    const float FILE_WIDTH = 1.5f;

    GameObject[,] putObjs = new GameObject[12, 7];
    Vector3[,] putPos = new Vector3[12,7]; 
    bool[,] isPut = new bool[12,7];

    //Player
    Player player;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        //設置系のものを生成しておく

        //TXTタレット
        GameObject txtTalletPool = new GameObject("TxtTalletPool");
        txtTalletPool.transform.parent = this.transform;
        for (int i = 0; i < 5; i++)
        {
            TxtTallet tallet = Instantiate(txtTallet, transform.position, Quaternion.identity);
            tallet.transform.parent = txtTalletPool.transform;
            txtTallets.Add(tallet);
        }
        //PNGバスター
        GameObject pngBusterPool = new GameObject("PngBusterPool");
        pngBusterPool.transform.parent = this.transform;
        for (int i = 0; i < 5; i++)
        {
            PngBuster buster = Instantiate(pngBuster,transform.position,Quaternion.identity);
            buster.transform.parent = pngBusterPool.transform;
            pngBusters.Add(buster);
        }
        //ハンマー
        GameObject hammerPool = new GameObject("HammerPool");
        hammerPool.transform.parent = this.transform;
        for (int i = 0; i < 5; i++)
        {
            Hammer hammer = Instantiate(hammerAttack, transform.position, Quaternion.identity);
            hammer.transform.parent = hammerPool.transform;
            hammers.Add(hammer);
        }
        //ロードサンダー
        GameObject loadTunderPool = new GameObject("LoadTunderPool");
        loadTunderPool.transform.parent = this.transform;
        for (int i = 0; i < 10; i++)
        {
            LoadTunder loadTunder = Instantiate(loadTunderAttack, transform.position, Quaternion.identity);
            loadTunder.transform.parent = loadTunderPool.transform;
            loadTunders.Add(loadTunder);
        }
        //ファイアウォール
        GameObject fireWallPostPool = new GameObject("FireWallPostPool");
        fireWallPostPool.transform.parent = this.transform;
        for (int i = 0; i < 3; i++)
        {
            FireWallPost post = Instantiate(fireWallPost, transform.position, Quaternion.identity);
            post.transform.parent = fireWallPostPool.transform;
            fireWallPosts.Add(post);

            FireWall fire = Instantiate(fireWall, transform.position, Quaternion.identity);
            fire.transform.parent = fireWallPostPool.transform;
            fireWalls.Add(fire);
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
