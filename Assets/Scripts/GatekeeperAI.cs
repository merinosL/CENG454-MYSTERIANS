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

    [Header("Detection")]
    public LayerMask playerLayer;
    private Transform playerTransform;
    private bool isChasing = false;
    private Rigidbody2D rb;

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

        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
        }

        if (isChasing)
        {
            LookAtPlayer();
        }
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

    void LookAtPlayer()
    {
        if (playerTransform.position.x > transform.position.x && transform.localScale.x < 0)
            Flip();
        else if (playerTransform.position.x < transform.position.x && transform.localScale.x > 0)
            Flip();
    }

    void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("<color=orange>GATEKEEPER LOG:</color> Received " + damage + " damage. Current HP: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("<color=yellow>GATEKEEPER LOG:</color> Entity destroyed. Dispatching death signal.");
        OnGatekeeperDeath?.Invoke();
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("<color=red>GATEKEEPER CONTACT:</color> Player hit detected. Sending 1 damage signal.");
            OnPlayerContact?.Invoke(1);
            
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                float pushDir = (collision.transform.position.x > transform.position.x) ? 1f : -1f;
                playerRb.linearVelocity = new Vector2(pushDir * 12f, 6f);
            }
        }
    }
}