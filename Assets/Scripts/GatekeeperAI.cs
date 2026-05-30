using System;
using UnityEngine;

public class GatekeeperAI : MonoBehaviour
{
    [Header("Health Settings")]
    public int health = 3;

    [Header("Movement Settings")]
    public float moveSpeed = 2.5f;
    public float detectionRange = 8f;
    public float stopDistance = 3.5f;

    [Header("Combat Settings")]
    public int damage = 1;
    public float attackCooldown = 1.5f; 
    private float lastAttackTime;

    private Transform playerTransform;
    private Rigidbody2D rb;
    private Animator anim;
    private bool isChasing = false;
    private bool isDead = false; 

    public static event Action OnGatekeeperDeath;
    public static event Action<int> OnPlayerContact;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) playerTransform = playerObj.transform;
    }

    void Update()
    {
        if (isDead || playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        
        if (distanceToPlayer <= detectionRange) isChasing = true;

        LookAtPlayer();
    }

    void FixedUpdate()
    {
        if (isDead) return;

        if (isChasing && playerTransform != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            
            if (distanceToPlayer > stopDistance)
            {
                anim.SetBool("isMoving", true);
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
            }
            else
            {
                anim.SetBool("isMoving", false);
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    Attack();
                }
            }
        } 
        else if (rb != null)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        anim.SetTrigger("Attack"); 
    }

    public void DealDamageEvent()
    {
        if (playerTransform == null || isDead) return;

        if (Vector2.Distance(transform.position, playerTransform.position) <= stopDistance + 1f)
        {
            OnPlayerContact?.Invoke(damage);

            PlayerHealth playerHealth = playerTransform.GetComponent<PlayerHealth>();
            if (playerHealth != null) playerHealth.TakeDamage(damage);

            Rigidbody2D playerRb = playerTransform.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector2.zero;
                float pushDir = (playerTransform.position.x > transform.position.x) ? 1f : -1f;
                playerRb.linearVelocity = new Vector2(pushDir * 20f, 5f);
            }
        }
    }

    void LookAtPlayer()
    {
        if (playerTransform.position.x > transform.position.x && transform.localScale.x > 0) Flip();
        else if (playerTransform.position.x < transform.position.x && transform.localScale.x < 0) Flip();
    }

    void Flip() => transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        anim.SetTrigger("GetHit"); 
        if (health <= 0) Die();
    }

    void Die()
    {
        isDead = true;
        anim.SetTrigger("Death");
        OnGatekeeperDeath?.Invoke();

        if (rb != null) 
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        
        Collider2D coll = GetComponent<Collider2D>();
        if (coll != null) coll.enabled = false;

        Destroy(gameObject, 1f); 
    }
}