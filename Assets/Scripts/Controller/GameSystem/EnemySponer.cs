using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySponer : MonoBehaviour,IGrabbable,ISelectable
{
    [SerializeField] public GameObject[] sponerEnemyPrefabs;
    [System.NonSerialized] public GameObject[] sponerEnemies;
    [System.NonSerialized] public EnemyBase[] sponerEnemyBases;

    float popTimer = 0;
    float popInterval = 0;
    int popNum = 0;
    [SerializeField] public float minPopInterval;
    [SerializeField] public float maxPopInterval;

    Transform sponerTrans;
    SpriteRenderer renderer;
    bool isSleep;

    bool isGrabed;
    Vector3 diffPlayerVec;
    PCFieldController pcFieldController => PCFieldController.instance;

    [System.NonSerialized] public GameObject phaseObjPool;

    // Start is called before the first frame update
    void Start()
    {
        sponerTrans = GetComponent<Transform>();
        renderer = GetComponent<SpriteRenderer>();

        sponerEnemies = new GameObject[sponerEnemyPrefabs.Length];
        sponerEnemyBases = new EnemyBase[sponerEnemyPrefabs.Length];
        for (int i = 0; i < sponerEnemyPrefabs.Length; i++)
        {
            GameObject enmey = Instantiate(sponerEnemyPrefabs[i],transform.position,Quaternion.identity);
            enmey.transform.parent = phaseObjPool.transform;
            sponerEnemies[i] = enmey;
            sponerEnemyBases[i] = enmey.GetComponent<EnemyBase>();
        }

        popInterval = Random.Range(minPopInterval,maxPopInterval);
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSleep)
        {
            popTimer += Time.deltaTime;
            if (popTimer >= popInterval) 
            {
                popTimer = 0;
                popInterval = Random.Range(minPopInterval, maxPopInterval);
                if (sponerEnemyBases[popNum].state == EnemyBase.State.Sleep)
                {
                    sponerEnemyBases[popNum].Initialize();
                    sponerEnemies[popNum].transform.position = sponerTrans.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
                }
                popNum++;
                if (popNum >= sponerEnemies.Length) 
                {
                    Reset();
                }
            }
        }
    }
    public void Initialize(Vector3 pos)
    {
        isSleep = false;
        renderer.enabled = true;
        sponerTrans.position = pos;
        isGrabed = false;
        sponerTrans.position = pcFieldController.GetPos(this.gameObject, sponerTrans.position);
        popNum = 0;

    }

    void Reset()
    {
        isSleep = true;
        renderer.enabled = false;
        pcFieldController.MovePos(this.gameObject);
    }

    public void Grabbing(Transform cursorTrans) 
    {
        if (!isGrabed) 
        {
            diffPlayerVec = sponerTrans.position - cursorTrans.position;
            pcFieldController.MovePos(this.gameObject);
        }
        sponerTrans.position = cursorTrans.position + diffPlayerVec;
        isGrabed = true;
    }

    public void Putting() 
    {
        isGrabed = false;
        sponerTrans.position = pcFieldController.GetPos(this.gameObject,sponerTrans.position);
    }
    public void Selected() { }

    public void Open()
    {
        for (int i = popNum; i < sponerEnemyBases.Length; i++)
        {
            if (sponerEnemyBases[i].state == EnemyBase.State.Sleep) 
            {
                sponerEnemyBases[i].Initialize();
                sponerEnemies[i].transform.position = sponerTrans.position + new Vector3(Random.Range(-2f,2f), Random.Range(-2f, 2f),0);
            }
        }
        Reset();
    }

    public void Delete()
    {
        
    }
}
