using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    [System.NonSerialized]public float hp;
    public float maxHp;

    [System.NonSerialized] public SpriteRenderer renderer;

    EnemyUIGenerator uiGenerator => EnemyUIGenerator.instance;
    [SerializeField] List<DamageDisplay> damageDisplays;
    [System.NonSerialized] public Image hpImage;

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


    public PCFieldController pcFieldController => PCFieldController.instance;
    public Server server;
    public Transform serverTrans;

    public Transform enemyTrans;
    public Rigidbody2D rb;

    public Vector3 enemyVelocity;

    const float ENEMY_SPEED = 2;

    const float ATTACK_RANGE_RADIUS = 2;
    const float ATTACK_INTERVAL = 0.5f;
    float attackTimer = 0;

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
    public virtual void Update()
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
            enemyVelocity = severDir * ENEMY_SPEED;
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
                if (serverVec.sqrMagnitude >= ATTACK_RANGE_RADIUS * ATTACK_RANGE_RADIUS)
                {
                    Debug.Log("ATTACK");
                }
                attackTimer = 0;
                state = State.StateDecide;
            }
        }
        else if (state == State.Stop)
        {

        }

        if (hp <= 0)
        {
            Reset();
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
}
