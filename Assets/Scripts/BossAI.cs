using UnityEngine;
using System;
using System.Collections.Generic;

public class BossAI : MonoBehaviour
{
    public int health = 5;
    public bool isAwake = false;

    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float projectileSpeed = 10f;

    [Header("Object Pool Settings")]
    public int poolSize = 10;
    private List<GameObject> bulletPool;

    private Transform playerTransform;
    private Rigidbody2D rb;
    private Animator anim;

    private bool isDead = false;
    private float nextFireTime;
    private bool facingRight = false;

    public static event Action OnBossDeath;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        GameObject pObj = GameObject.FindGameObjectWithTag("Player");
        if (pObj != null) playerTransform = pObj.transform;

        bulletPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(projectilePrefab);
            obj.SetActive(false);
            bulletPool.Add(obj);
        }
    }

    void OnEnable()
    {
        GatekeeperAI.OnGatekeeperDeath += WakeUp;
    }

    void OnDisable()
    {
        GatekeeperAI.OnGatekeeperDeath -= WakeUp;
    }

    void WakeUp()
    {
        isAwake = true;
    }

    void Update()
    {
        if (isDead || !isAwake || playerTransform == null) return;

        LookAtPlayer();

        if (Time.time >= nextFireTime)
        {
            TriggerAttack();
            nextFireTime = Time.time + fireRate;
        }
    }

    void TriggerAttack()
    {
        if (isDead) return;
        anim.SetTrigger("Attack");
    }

    private GameObject GetPooledBullet()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            if (!bulletPool[i].activeInHierarchy)
            {
                return bulletPool[i];
            }
        }
        GameObject newObj = Instantiate(projectilePrefab);
        newObj.SetActive(false);
        bulletPool.Add(newObj);
        return newObj;
    }

    public void Shoot()
    {
        if (playerTransform == null || isDead) return;

        if (projectilePrefab != null && firePoint != null)
        {
            GameObject bullet = GetPooledBullet();

            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = Quaternion.identity;
            bullet.SetActive(true);

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

            if (bulletRb != null)
            {
                Vector2 direction = (playerTransform.position - firePoint.position).normalized;
                bulletRb.linearVelocity = direction * projectileSpeed;
                bullet.transform.right = -direction;
            }

            Collider2D bulletCol = bullet.GetComponent<Collider2D>();
            Collider2D bossCol = GetComponent<Collider2D>();
            if (bulletCol != null && bossCol != null)
            {
                Physics2D.IgnoreCollision(bulletCol, bossCol);
            }
        }
    }

    void LookAtPlayer()
    {
        if (playerTransform.position.x > transform.position.x && !facingRight) Flip();
        else if (playerTransform.position.x < transform.position.x && facingRight) Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        health -= damage;

        if (health <= 0) Die();
        else anim.SetTrigger("GetHit");
    }

    void Die()
    {
        isDead = true;
        anim.ResetTrigger("Attack");
        anim.ResetTrigger("GetHit");
        anim.SetTrigger("Death");
        OnBossDeath?.Invoke();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        Collider2D coll = GetComponent<Collider2D>();
        if (coll != null) coll.enabled = false;

        Destroy(gameObject, 2.5f);
    }
}