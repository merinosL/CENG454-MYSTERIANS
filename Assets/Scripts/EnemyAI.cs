using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Chase,
        Dead
    }

    public State currentState = State.Patrol;

    [Header("Can Ayarları")]
    public int health = 1;

    [Header("Hareket Ayarları")]
    public float moveSpeed = 2f;
    public float chaseSpeed = 3.5f;
    public float patrolRadius = 5f;

    [Header("Sensörler")]
    public Transform edgeCheck;
    public float rayDistance = 2f;
    public float visionRange = 5f;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    
    private Animator animator;
    private Rigidbody2D rb;
    private bool movingRight = true;
    public Transform player;

    private float leftBoundary;
    private float rightBoundary;

    public static event Action<int> OnPlayerContact;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        CalculateNewPatrolArea();
    }

    void Update()
    {
        if (currentState == State.Dead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (currentState == State.Patrol)
        {
            CheckForPlayer();
        }
        else if (currentState == State.Chase)
        {
            CheckIfPlayerLost();
        }
    }

    void FixedUpdate()
    {
        if (currentState == State.Dead) return;

        if (currentState == State.Patrol)
        {
            PatrolLogic();
        }
        else if (currentState == State.Chase)
        {
            ChaseLogic();
        }
    }

    void PatrolLogic()
    {
        animator.SetBool("isMoving", true);
        rb.linearVelocity = new Vector2((movingRight ? moveSpeed : -moveSpeed), rb.linearVelocity.y);

        RaycastHit2D groundInfo = Physics2D.Raycast(edgeCheck.position, Vector2.down, rayDistance, groundLayer);

        if (groundInfo.collider == false || 
            (movingRight && transform.position.x >= rightBoundary) || 
            (!movingRight && transform.position.x <= leftBoundary))
        {
            Flip();
        }
    }

    void ChaseLogic()
    {
        animator.SetBool("isMoving", true);
        if (player == null) return;

        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(direction * chaseSpeed, rb.linearVelocity.y);

        if (direction > 0 && !movingRight) Flip();
        else if (direction < 0 && movingRight) Flip();
    }

    void CheckForPlayer()
    {
        Vector2 visionDirection = movingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, visionDirection, visionRange, playerLayer);

        if (hit.collider != null)
        {
            currentState = State.Chase;
        }
    }

    void CheckIfPlayerLost()
    {
        animator.SetBool("isMoving", false);
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > visionRange + 2f)
        {
            currentState = State.Patrol;
            CalculateNewPatrolArea();
        }
    }

    void CalculateNewPatrolArea()
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
        if (currentState == State.Dead) return;

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
        animator.SetTrigger("GetHit");
        health -= damage;
        
        if (health <= 0 && currentState != State.Dead)
        {
            Die();
        }
        Debug.Log("HIT!");
        animator.SetTrigger("GetHit");
    }

    void Die()
    {
        animator.SetTrigger("Death");
        currentState = State.Dead;
        transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.z);
        GetComponent<Collider2D>().enabled = false;
    }

    private void OnDrawGizmos()
    {
        if (edgeCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(edgeCheck.position, edgeCheck.position + Vector3.down * rayDistance);
        }

        Gizmos.color = Color.yellow;
        Vector2 visionDirection = movingRight ? Vector2.right : Vector2.left;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + visionDirection * visionRange);

        float drawLeft = Application.isPlaying ? leftBoundary : transform.position.x - patrolRadius;
        float drawRight = Application.isPlaying ? rightBoundary : transform.position.x + patrolRadius;
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector2(drawLeft, transform.position.y - 1), new Vector2(drawLeft, transform.position.y + 1));
        Gizmos.DrawLine(new Vector2(drawRight, transform.position.y - 1), new Vector2(drawRight, transform.position.y + 1));
    }
}