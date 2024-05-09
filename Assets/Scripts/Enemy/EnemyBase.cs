using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    [System.NonSerialized]public float hp;
    public float maxHp;

    float damage = 5;

    float deathTimer = 0; 
    const float DEATH_INTERVAL = 0.5f;

    [System.NonSerialized] public SpriteRenderer renderer;

    EnemyUIGenerator uiGenerator => EnemyUIGenerator.instance;
    [SerializeField] List<DamageDisplay> damageDisplays;
    [System.NonSerialized] public Image hpImage;

    public Animator animator;

    public enum State
    {
        None,
        StateDecide,
        Sleep,
        Grabed,
        GoServer,
        Attack,
        Stop,
        Death
    }
    public State state;

    public Element haveElement;


    public ComboController comboController => ComboController.instance;
    public PCFieldController pcFieldController => PCFieldController.instance;
    public ExpController expController => ExpController.instance;
    public Server server;
    [System.NonSerialized] public Transform serverTrans;

    [System.NonSerialized] public Transform enemyTrans;
    [System.NonSerialized] public Rigidbody2D rb;

    [System.NonSerialized] public Vector3 enemyVelocity;

    public float enemySpeed = 1;

    public const float ATTACK_RANGE_RADIUS = 2;
    public const float ATTACK_INTERVAL = 2f;
    public float attackTimer = 0;

    [SerializeField] bool haveFile;

    // Start is called before the first frame update
    public virtual void Start()
    {
        renderer = GetComponent<SpriteRenderer>();

        Image[] images = GetComponentsInChildren<Image>();
        for (int i = 0; i < images.Length; i++)
        {
            if (images[i].type == Image.Type.Filled)
            {
                hpImage = images[i];
                break;
            }
        }

        server = pcFieldController.server;
        serverTrans = server.transform;
        enemyTrans = this.GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();

        Reset();
    }

    // Update is called once per frame
    public void UpdateAction()
    {
        HpDisplay();

        enemyVelocity = Vector3.zero;
        Vector2 serverVec = (serverTrans.position - enemyTrans.position);
        Vector2 severDir = serverVec.normalized;

        if (state == State.None)
        {

        }
        else if (state == State.StateDecide)
        {
            //仮
            state = State.GoServer;
            attackTimer = 0;
        }
        else if (state == State.Sleep)
        {

        }
        else if (state == State.Grabed)
        {

        }
        else if (state == State.GoServer)
        {
            enemyVelocity = severDir * enemySpeed;
            if (serverVec.sqrMagnitude <= ATTACK_RANGE_RADIUS * ATTACK_RANGE_RADIUS)
            {
                state = State.Attack;
            }
        }
        else if (state == State.Attack)
        {
            if (attackTimer <= ATTACK_INTERVAL)
            {
                attackTimer += Time.deltaTime;
            }
            if (attackTimer >= ATTACK_INTERVAL)
            {
                if (serverVec.sqrMagnitude <= ATTACK_RANGE_RADIUS * ATTACK_RANGE_RADIUS)
                {
                    server.AddDamage(damage);
                }
                attackTimer = 0;
                state = State.StateDecide;
            }
        }
        else if (state == State.Stop)
        {

        }
        else if (state == State.Death) 
        {
            enemyVelocity = Vector2.zero;
            deathTimer += Time.deltaTime;
            if (deathTimer >= DEATH_INTERVAL)
            {
                Died();
                Reset();
                deathTimer = 0;
            }
        }

        if (hp <= 0 && state != State.Sleep)
        {
            state = State.Death;
        }

    }

    public void Initialize()
    {
        state = State.StateDecide;
        renderer.enabled = true;
        hp = maxHp;

        GameObject canvas = transform.GetChild(0).gameObject;
        canvas.SetActive(true);
    }

    public void Reset()
    {
        state = State.Sleep;
        renderer.enabled = false;

        GameObject canvas = transform.GetChild(0).gameObject;
        canvas.SetActive(false);

        transform.position = new Vector3(0,50,0);
    }

    public void Died() 
    {
        //仮
        if (haveFile) 
        {
            List<TreasureFile> files = pcFieldController.treasureFiles;
            for (int i = 0; i < files.Count; i++)
            {
                if (files[i].isSleep)
                {
                    files[i].Initialize(enemyTrans.position);
                    break;
                }
            }
        }
        else 
        {
            for (int i = 0; i < 3; i++)
            {
                expController.InitializeExpS(enemyTrans.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0));
            }
        }
        
    }

    /// <summary>
    /// ダメージの数値を表記する
    /// </summary>
    /// <param name="pos">表示場所</param>
    /// <param name="damage">ダメージ量</param>
    public void DamageDisplay(Vector3 pos, float damage)
    {
        damageDisplays = uiGenerator.damageDisplays;

        for (int i = 0; i < damageDisplays.Count; i++)
        {
            if (damageDisplays[i].isSleep)
            {
                damageDisplays[i].Initialize(pos,damage);
                break;
            }
        }
    }

    /// <summary>
    /// HPを表示する
    /// </summary>
    public void HpDisplay() 
    {
        hpImage.fillAmount = (hp/maxHp);
    }

    public void EnemyAnimation() 
    {
        int num = 1;
        if (enemyTrans.position.x >= serverTrans.position.x) 
        {
            num = -1;
        }
        else
        {
            num = 1;
        }
        enemyTrans.localScale = new Vector3(num * Mathf.Abs(enemyTrans.localScale.x), enemyTrans.localScale.y, enemyTrans.localScale.z);
        if (state == State.Death) 
        {
            animator.SetInteger("animNum",2);
        }
        else if (state == State.Attack)
        {
            animator.SetInteger("animNum", 1);
        }
        else
        {
            animator.SetInteger("animNum", 0);
        }
    }
}
