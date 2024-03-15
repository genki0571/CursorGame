using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataFileBullet : MonoBehaviour
{
    const float BULLET_SPEED = 5f;
    const float BULLET_DAMAGE = 10;
    const float BULLET_INTERVAL = 5;

    float bulletTimer = 0;

    public bool isSleep;
    Transform trans;
    Rigidbody2D rb;
    SpriteRenderer renderer;

    Vector3 bulletDir;

    private void Awake()
    {
        trans = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        isSleep = true;
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

            rb.velocity = bulletDir * BULLET_SPEED;
        }
    }

    void Reset()
    {
        renderer.enabled = false;
        bulletTimer = 0;
        trans.localPosition = Vector3.zero;
        rb.velocity = Vector3.zero;
        isSleep = true;
    }

    public void Initialize(Vector3 dir)
    {
        bulletDir = dir;
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
