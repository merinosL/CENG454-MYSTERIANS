using System;
using UnityEngine;

public class RangedEnemyAI : MonoBehaviour
{
    [Header("Health Settings")]
    public int health = 2;

    [Header("Movement Settings")]
    public float moveSpeed = 1.5f;
    public float patrolRadius = 4f;

    [Header("Damage Settings")]
    public int contactDamage = 1;
    public float damageCooldown = 1f;
    private float lastDamageTime;

    [Header("Sensors")]
    public Transform edgeCheck;
    public float rayDistance = 2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D enemyCollider;
    private bool movingRight = true;
    private float leftBoundary;
    private float rightBoundary;
    private bool isDead = false;

    public static event Action<int> OnPlayerContact;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();
        CalculatePatrolArea();
    }

    void FixedUpdate()
    {
        if (isDead) return;
        PatrolLogic();
    }

    void PatrolLogic()
    {
        if (anim != null) anim.SetBool("isMoving", true);

        float targetVelocityX = movingRight ? moveSpeed : -moveSpeed;
        rb.linearVelocity = new Vector2(targetVelocityX, rb.linearVelocity.y);

        bool isGrounded = false;
        if (edgeCheck != null)
        {
            isGrounded = Physics2D.Raycast(edgeCheck.position, Vector2.down, rayDistance, groundLayer);
        }

        if (!isGrounded || 
            (movingRight && transform.position.x >= rightBoundary) || 
            (!movingRight && transform.position.x <= leftBoundary))
        {
            Flip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            DealContactDamage(collision.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            DealContactDamage(collision.gameObject);
        }
    }

    private void DealContactDamage(GameObject playerObj)
    {
        if (Time.time < lastDamageTime + damageCooldown) return;

        lastDamageTime = Time.time;

        OnPlayerContact?.Invoke(contactDamage);

        PlayerHealth playerHealth = playerObj.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(contactDamage);
        }

        Rigidbody2D playerRb = playerObj.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
            float pushDir = (playerObj.transform.position.x > transform.position.x) ? 1f : -1f;
            playerRb.linearVelocity = new Vector2(pushDir * 10f, 5f); 
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

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            if (anim != null) anim.SetTrigger("GetHit");
        }
    }

    void Die()
    {
        isDead = true;
        if (anim != null) anim.SetTrigger("Death");
        
        if (enemyCollider != null) enemyCollider.enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        
        Destroy(gameObject, 1f);
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