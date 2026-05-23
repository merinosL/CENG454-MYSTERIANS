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

    public float knockbackForce = 25f; 
    public float verticalForce = 10f;

    public static event Action OnGatekeeperDeath;
    public static event Action<int> OnPlayerContact;

    private Transform playerTransform;
    private Rigidbody2D rb;
    private bool isChasing = false;

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
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("<color=red>GATEKEEPER CONTACT:</color> Pushing player away.");
                OnPlayerContact?.Invoke(1);

                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    // Önce tüm hızları sıfırla (Çok önemli!)
                    playerRb.linearVelocity = Vector2.zero;

                    float pushDir = (collision.transform.position.x > transform.position.x) ? 1f : -1f;
                    
                    // DEĞİŞİKLİK: Yatay gücü artırdık (30), dikey gücü azalttık (4)
                    // Böylece üstüne çıkmak yerine yerden süpürülerek uzaklaşacak
                    float finalKnockback = 30f; 
                    float finalVertical = 4f; 

                    playerRb.linearVelocity = new Vector2(pushDir * finalKnockback, finalVertical);
                }
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