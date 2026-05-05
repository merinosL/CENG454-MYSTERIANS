using System;
using UnityEngine;

public class RangedEnemyAI : MonoBehaviour
{
    [Header("Can Ayarları")]
    public int health = 2;

    [Header("Hareket Ayarları")]
    public float moveSpeed = 1.5f;
    public float patrolRadius = 4f;

    [Header("Atış Ayarları")]
    public GameObject projectilePrefab; 
    public Transform firePoint;         
    public float fireRate = 3.5f;       
    private float nextFireTime;

    [Header("Sensörler")]
    public Transform edgeCheck;
    public float rayDistance = 2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Collider2D enemyCollider; 
    private bool movingRight = true;
    private float leftBoundary;
    private float rightBoundary;

    public static event Action<int> OnPlayerContact;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>(); 
        CalculatePatrolArea();
        nextFireTime = Time.time + fireRate;
    }

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FixedUpdate()
    {
        PatrolLogic();
    }

    void PatrolLogic()
    {
        rb.linearVelocity = new Vector2((movingRight ? moveSpeed : -moveSpeed), rb.linearVelocity.y);

        RaycastHit2D groundInfo = Physics2D.Raycast(edgeCheck.position, Vector2.down, rayDistance, groundLayer);

        if (groundInfo.collider == null || 
            (movingRight && transform.position.x >= rightBoundary) || 
            (!movingRight && transform.position.x <= leftBoundary))
        {
            Flip();
        }
    }

    void Shoot()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, transform.rotation);
            
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                float bulletSpeed = 7f;
                float facingDirection = movingRight ? 1f : -1f; 
                bulletRb.linearVelocity = new Vector2(facingDirection * bulletSpeed, 0);
            }

            Collider2D bulletCollider = bullet.GetComponent<Collider2D>();
            if (bulletCollider != null && enemyCollider != null)
            {
                Physics2D.IgnoreCollision(bulletCollider, enemyCollider);
            }

            Destroy(bullet, 3f); 
        }
    }

    void CalculatePatrolArea()
    {
        leftBoundary = transform.position.x - patrolRadius;
        rightBoundary = transform.position.x + patrolRadius;
    }

    void Flip()
    {
        movingRight = !movingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnPlayerContact?.Invoke(1);

            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                float pushDirectionX = (collision.transform.position.x > transform.position.x) ? 1f : -1f;
                Vector2 knockback = new Vector2(pushDirectionX * 10f, 5f);
                playerRb.linearVelocity = knockback;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (edgeCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(edgeCheck.position, edgeCheck.position + Vector3.down * rayDistance);
        }

        float drawLeft = Application.isPlaying ? leftBoundary : transform.position.x - patrolRadius;
        float drawRight = Application.isPlaying ? rightBoundary : transform.position.x + patrolRadius;
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector2(drawLeft, transform.position.y - 1), new Vector2(drawLeft, transform.position.y + 1));
        Gizmos.DrawLine(new Vector2(drawRight, transform.position.y - 1), new Vector2(drawRight, transform.position.y + 1));
    }
}