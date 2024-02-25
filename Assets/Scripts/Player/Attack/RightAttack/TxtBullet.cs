using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TxtBullet : MonoBehaviour
{
    const float BULLET_SPEED = 5f;
    const float BULLET_DAMAGE = 10;
    const float BULLET_INTERVAL = 10;

    float bulletTimer = 0;

    public bool isSleep;
    Transform trans;
    Rigidbody2D rb;
    SpriteRenderer renderer;

    GameObject aimEnemy;

    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSleep) 
        {
            bulletTimer += Time.deltaTime;
            if (bulletTimer >= BULLET_INTERVAL) 
            {
                Reset();
            }

            if (aimEnemy)
            {
                Vector3 bulletVelocity = (aimEnemy.transform.position - trans.position).normalized;
                rb.velocity = bulletVelocity * BULLET_SPEED;
            }
        }
        
    }

    void Reset() 
    {
        renderer.enabled = false;
        aimEnemy = null;
        bulletTimer = 0;
        trans.localPosition = Vector3.zero;
        rb.velocity = Vector3.zero;
        isSleep = true;
    }

    public void Initialize(GameObject enemy)
    {
        aimEnemy = enemy;
        renderer.enabled = true;
        isSleep = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isSleep) 
        {
            IDamagable damagable = collision.GetComponent<IDamagable>();
            if (damagable != null) 
            {
                damagable.AddDamage(BULLET_DAMAGE);
                Reset();
            }
        }
    }
}
