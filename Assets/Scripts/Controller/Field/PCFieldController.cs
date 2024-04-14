using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCFieldController : MonoBehaviour
{
    public static PCFieldController instance;

    PlayerSkill playerSkill => PlayerSkill.instance;

    public Server server;

    //Prefabs
    [SerializeField] FallText fallText;
    [SerializeField] TxtTallet txtTallet;
    [SerializeField] PngBuster pngBuster;
    [SerializeField] ZipFile zipFile;
    [SerializeField] Hammer hammerAttack;
    [SerializeField] LoadThunder loadThunderAttack;
    [SerializeField] FireWallPost fireWallPost;
    [SerializeField] FireWall fireWall;
    [SerializeField] public GameObject windArea;
    [SerializeField] public GameObject electricShock;
    [SerializeField] public GameObject storm;
    [SerializeField] public GameObject fireArea;
    [SerializeField] public GameObject explotion;

    [SerializeField] public GameObject selectTargetPool;
    [SerializeField] public GameObject targetPool;
    [SerializeField] public GameObject enemyAttackPool;

    //Pool
    public List<FallText> fallTexts = new List<FallText>();
    public List<TxtTallet> txtTallets = new List<TxtTallet>();
    public List<PngBuster> pngBusters = new List<PngBuster>();
    public List<Hammer> hammers = new List<Hammer>();
    public List<LoadThunder> loadThunders = new List<LoadThunder>();
    public List<FireWallPost> fireWallPosts = new List<FireWallPost>();
    public List<FireWall> fireWalls = new List<FireWall>();
    public List<ZipFile> zipFiles = new List<ZipFile>();
    public List<WeekPoint> weekPoints = new List<WeekPoint>();
    public List<Transform> selectTargets;
    public List<EnemyAttack> enemyAttacks = new List<EnemyAttack>();

    [SerializeField] Camera camera;
    Vector3 cameraLeftUpperPos;
    Vector3 fileLeftUpperPos;
    const float FILE_HEIGHT = 1;
    const float FILE_WIDTH = 1.5f;

    GameObject[,] putObjs = new GameObject[12, 7];
    Vector3[,] putPos = new Vector3[12,7]; 
    bool[,] isPut = new bool[12,7];

    public float volumeNum;
    public PowerMode powerMode;
    public bool isAirlineMode;
    public bool isInternetConnect;
    public FontType fontType;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        //設置系のものを生成しておく
        if (playerSkill.rightLevel >= 0)
        {
            //FallText
            GameObject fallTextPool = new GameObject("FallTextPool");
            fallTextPool.transform.parent = this.transform;
            fallTextPool.transform.localPosition = Vector3.zero;
            for (int i = 0; i < 3; i++)
            {
                FallText text = Instantiate(fallText, transform.position, Quaternion.identity);
                text.transform.parent = fallTextPool.transform;
                fallTexts.Add(text);
            }
        }
        if (playerSkill.rightLevel >= 1 && playerSkill.rightSkill == RightSkill.Installation)
        {
            //TXTタレット
            GameObject txtTalletPool = new GameObject("TxtTalletPool");
            txtTalletPool.transform.parent = this.transform;
            for (int i = 0; i < 5; i++)
            {
                TxtTallet tallet = Instantiate(txtTallet, transform.position, Quaternion.identity);
                tallet.transform.parent = txtTalletPool.transform;
                txtTallets.Add(tallet);
            }
        }
        if (playerSkill.rightLevel >= 2 && playerSkill.rightSkill == RightSkill.Installation)
        {
            //PNGバスター
            GameObject pngBusterPool = new GameObject("PngBusterPool");
            pngBusterPool.transform.parent = this.transform;
            for (int i = 0; i < 5; i++)
            {
                PngBuster buster = Instantiate(pngBuster, transform.position, Quaternion.identity);
                buster.transform.parent = pngBusterPool.transform;
                pngBusters.Add(buster);
            }
        }
        if (playerSkill.rightLevel >= 3 && playerSkill.rightSkill == RightSkill.Installation)
        {
            //ZIPファイル
            GameObject zipFilePool = new GameObject("ZipFilePostPool");
            zipFilePool.transform.parent = this.transform;
            for (int i = 0; i < 3; i++)
            {
                ZipFile zip = Instantiate(zipFile, transform.position, Quaternion.identity);
                zip.transform.parent = zipFilePool.transform;
                zipFiles.Add(zip);
            }
        }
        if (playerSkill.rightSkill == RightSkill.FireWall) 
        {
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
        }

        //LeftClick
        if (playerSkill.leftSkill == LeftSkill.Hammer)
        {
            //ハンマー
            GameObject hammerPool = new GameObject("HammerPool");
            hammerPool.transform.parent = this.transform;
            for (int i = 0; i < 10; i++)
            {
                Hammer hammer = Instantiate(hammerAttack, transform.position, Quaternion.identity);
                hammer.transform.parent = hammerPool.transform;
                hammers.Add(hammer);
            }
        }
        if (playerSkill.leftSkill == LeftSkill.LoadThunder)
        {
            //ロードサンダー
            GameObject loadThunderPool = new GameObject("LoadThunderPool");
            loadThunderPool.transform.parent = this.transform;
            for (int i = 0; i < 15; i++)
            {
                LoadThunder loadThunder = Instantiate(loadThunderAttack, transform.position, Quaternion.identity);
                loadThunder.transform.parent = loadThunderPool.transform;
                loadThunders.Add(loadThunder);
            }
        }
        

        //WeekPoint
        WeekPoint[] points = targetPool.GetComponentsInChildren<WeekPoint>();
        for (int i = 0; i < points.Length; i++)
        {
            weekPoints.Add(points[i]);
        }

        //SelectTarget
        Transform[] targets = selectTargetPool.GetComponentsInChildren<Transform>();
        for (int i = 0; i < targets.Length; i++)
        {
            selectTargets.Add(targets[i]);
        }
        selectTargets.Remove(selectTargetPool.transform);

        //EnemyAttack
        EnemyAttack[] attacks = enemyAttackPool.GetComponentsInChildren<EnemyAttack>();
        for (int i = 0; i < attacks.Length; i++)
        {
            enemyAttacks.Add(attacks[i]);
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
