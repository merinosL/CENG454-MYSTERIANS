using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State { Patrol, Chase, Dead }
    public State currentState = State.Patrol;

    [Header("Health Settings")]
    public int health = 1;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float chaseSpeed = 3.5f;
    public float patrolRadius = 5f;
    public float stopDistance = 1.2f;

    [Header("Attack Settings")]
    public float attackCooldown = 1.5f;
    private float lastAttackTime;
    private bool isAttacking = false;

    [Header("Sensors")]
    public Transform edgeCheck;
    public float rayDistance = 2f;
    public float visionRange = 5f;
    public float verticalTolerance = 1.5f;
    public LayerMask groundLayer;
    
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
        if (currentState == State.Dead || player == null || !player.gameObject.activeInHierarchy) return;
        if (isAttacking) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        float verticalDifference = Mathf.Abs(player.position.y - transform.position.y);

        bool canSeePlayer = (distanceToPlayer <= visionRange) && (verticalDifference <= verticalTolerance);

        if (currentState == State.Patrol && canSeePlayer)
        {
            currentState = State.Chase;
        }
        else if (currentState == State.Chase && !canSeePlayer)
        {
            currentState = State.Patrol;
            CalculateNewPatrolArea();
        }
    }

    void FixedUpdate()
    {
        if (currentState == State.Dead || isAttacking)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        if (player == null || !player.gameObject.activeInHierarchy)
        {
            currentState = State.Patrol;
        }

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
        
        bool isGrounded = false;
        if (edgeCheck != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(edgeCheck.position, Vector2.down, rayDistance, groundLayer);
            isGrounded = (hit.collider != null);
        }

        if (!isGrounded || 
            (movingRight && transform.position.x >= rightBoundary) || 
            (!movingRight && transform.position.x <= leftBoundary))
        {
            Flip();
        }

        float currentSpeed = movingRight ? moveSpeed : -moveSpeed;
        rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);
    }

    void ChaseLogic()
    {
        if (player == null) return;

        float distanceX = Mathf.Abs(player.position.x - transform.position.x);

        if (distanceX > stopDistance)
        {
            animator.SetBool("isMoving", true);
            float direction = Mathf.Sign(player.position.x - transform.position.x);
            
            if ((direction > 0 && !movingRight) || (direction < 0 && movingRight))
            {
                Flip();
            }

            rb.linearVelocity = new Vector2(direction * chaseSpeed, rb.linearVelocity.y);
        }
        else
        {
            animator.SetBool("isMoving", false);
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                TriggerAttack();
            }
        }
    }

    void TriggerAttack()
    {
        isAttacking = true; 
        animator.SetTrigger("Attack");
        lastAttackTime = Time.time;    
    }

    public void DealDamageDuringAnimation() 
    {
        if (player == null) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= 2.5f) 
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
            
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector2.zero;
                float pushDir = (player.position.x > transform.position.x) ? 1f : -1f;
                playerRb.linearVelocity = new Vector2(pushDir * 8f, 4f); 
            }
        }
    }

    public void OnAttackAnimationFinished()
    {
        isAttacking = false;
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

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && currentState != State.Dead) Die();
        else animator.SetTrigger("GetHit");
    }

    void Die()
    {
        currentState = State.Dead;
        SoundManager.Instance.PlaySound3D("EnemyDeath", transform.position);
        animator.SetTrigger("Death");
        
        GetComponent<Collider2D>().enabled = false;
        
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
    }
}