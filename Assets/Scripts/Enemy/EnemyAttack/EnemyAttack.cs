using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    const int DAMAGE = 1;
    const float LIFE_INTERVAL = 10;

    float lifeTimer = 0;

    public bool isSleep;
    Transform attackTrans;
    Rigidbody2D rb;
    SpriteRenderer renderer;

    PlayerState playerState => PlayerState.instance;

    private void Awake()
    {
        attackTrans = this.transform;
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        isSleep = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSleep)
        {
            lifeTimer += Time.deltaTime;
            if (lifeTimer >= LIFE_INTERVAL)
            {
                Reset();
            }
        }
    }

    void Reset()
    {
        renderer.enabled = false;
        lifeTimer = 0;
        attackTrans.position = new Vector3(0,70,0);
        rb.velocity = Vector3.zero;
        isSleep = true;
    }

    public void Initialize(Vector3 pos,Vector3 dir,float speed)
    {
        attackTrans.position = pos;
        rb.velocity = dir * speed;
        renderer.enabled = true;
        isSleep = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isSleep)
        {
            if (collision.transform.tag == "Player") 
            {
                playerState.AddDamage(DAMAGE);
                playerState.GetStan(1f);
                Reset();
            }
        }
    }
}
