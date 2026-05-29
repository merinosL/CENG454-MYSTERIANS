using System;
using UnityEngine;

public class GatekeeperAI : MonoBehaviour
{
    [Header("Health Settings")]
    public int health = 3;

    [Header("Movement Settings")]
    public float moveSpeed = 2.5f;
    public float detectionRange = 8f;
    public float stopDistance = 1.2f;

    [Header("Combat Settings")]
    public int damage = 1;
    public float attackCooldown = 1.5f; 
    private float lastAttackTime;

    private Transform playerTransform;
    private Rigidbody2D rb;
    private bool isChasing = false;

    public static event Action OnGatekeeperDeath;
    public static event Action<int> OnPlayerContact;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) playerTransform = playerObj.transform;
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        
        if (distanceToPlayer <= detectionRange) isChasing = true;

        if (isChasing) LookAtPlayer();
    }

    void FixedUpdate()
    {
        if (isChasing && playerTransform != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            
            if (distanceToPlayer > stopDistance)
            {
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
            }
            else
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack(collision.gameObject);
        }
    }

    void Attack(GameObject playerObj)
    {
        lastAttackTime = Time.time;
        OnPlayerContact?.Invoke(damage);

        PlayerHealth playerHealth = playerObj.GetComponent<PlayerHealth>();
        if (playerHealth != null) playerHealth.TakeDamage(damage);

        Rigidbody2D playerRb = playerObj.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
            float pushDir = (playerObj.transform.position.x > transform.position.x) ? 1f : -1f;
            playerRb.linearVelocity = new Vector2(pushDir * 20f, 5f);
        }
    }

    void LookAtPlayer()
    {
        if (playerTransform.position.x > transform.position.x && transform.localScale.x < 0) Flip();
        else if (playerTransform.position.x < transform.position.x && transform.localScale.x > 0) Flip();
    }

    void Flip() => transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    void Die()
    {
        OnGatekeeperDeath?.Invoke();
        Destroy(gameObject);
    }
}