using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("Boss Settings")]
    public int health = 5;
    public int damageToPlayer = 2;
    public bool isAwake = false;
    
    [Header("Shooting Settings (Final Phase Only)")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1.2f;
    private float nextFireTime;

    [Header("Sensors")]
    public Transform groundCheck;
    public Transform edgeCheck;
    public float groundCheckRadius = 0.4f;
    public float edgeRayDistance = 2f;
    public float patrolRadius = 6f;
    public LayerMask groundLayer;

    private float currentJumpX;
    private float currentJumpY;
    private float currentJumpDelay;

    private Rigidbody2D rb;
    private Transform player;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float jumpTimer;
    private bool isGrounded;
    private bool facingRight = true;
    private float leftBoundary;
    private float rightBoundary;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
        leftBoundary = transform.position.x - patrolRadius;
        rightBoundary = transform.position.x + patrolRadius;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        UpdatePhaseStats();
        jumpTimer = currentJumpDelay;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        animator.SetBool("isJumping", !isGrounded);

        if (isGrounded && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            jumpTimer -= Time.deltaTime;
            
            if (jumpTimer <= 0f)
            {
                animator.SetBool("isRunning", true);
                if (isAwake && health <= 3) 
                {
                    LookAtPlayer();
                    CheckAndJump(false);
                }
                else 
                {
                    CheckAndJump(true);
                }
                
                jumpTimer = currentJumpDelay;
            }
        }

        if (isAwake && health == 1 && Time.time >= nextFireTime)
        {
            animator.SetTrigger("Attack");
            CrazyShoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void UpdatePhaseStats()
    {
        if (health >= 4)
        {
            currentJumpX = 5f;
            currentJumpY = 8f;
            currentJumpDelay = 3f;
            spriteRenderer.color = Color.white;
        }
        else if (health == 3)
        {
            currentJumpX = 12f;
            currentJumpY = 14f;
            currentJumpDelay = 2f;
            spriteRenderer.color = new Color(1f, 0.5f, 0f);
        }
        else if (health == 2)
        {
            currentJumpX = 6f;
            currentJumpY = 10f;
            currentJumpDelay = 4f;
            spriteRenderer.color = Color.gray;
        }
        else if (health == 1)
        {
            currentJumpX = 16f;
            currentJumpY = 17f;
            currentJumpDelay = 1.3f;
            spriteRenderer.color = Color.red;
        }
    }

    void LookAtPlayer()
    {
        if (player == null) return;
        if (player.position.x > transform.position.x && !facingRight) Flip();
        else if (player.position.x < transform.position.x && facingRight) Flip();
    }

    void CheckAndJump(bool checkBoundaries)
    {
        RaycastHit2D edgeInfo = Physics2D.Raycast(edgeCheck.position, Vector2.down, edgeRayDistance, groundLayer);
        bool outOfBounds = false;

        if (checkBoundaries)
        {
            if (facingRight && transform.position.x >= rightBoundary) outOfBounds = true;
            else if (!facingRight && transform.position.x <= leftBoundary) outOfBounds = true;
        }

        if (edgeInfo.collider != null && !outOfBounds)
        {
            JumpAttack();
        }
        else
        {
            Flip();
        }
    }

    void JumpAttack()
    {
        float direction = facingRight ? 1f : -1f;
        animator.SetTrigger("Attack");
        rb.linearVelocity = Vector2.zero;
        Vector2 jumpVector = new Vector2(direction * currentJumpX, currentJumpY);
        rb.AddForce(jumpVector * rb.mass, ForceMode2D.Impulse);
    }

    void CrazyShoot()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            FireBullet(Vector2.right);
            FireBullet(Vector2.left);
            FireBullet(new Vector2(1f, 1f).normalized);
            FireBullet(new Vector2(-1f, 1f).normalized);
        }
    }

    void FireBullet(Vector2 direction)
    {
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        
        if (bulletRb != null)
        {
            float projectileSpeed = 10f;
            bulletRb.linearVelocity = direction * projectileSpeed;
        }

        Collider2D bulletCol = bullet.GetComponent<Collider2D>();
        Collider2D bossCol = GetComponent<Collider2D>();
        if (bulletCol != null && bossCol != null) Physics2D.IgnoreCollision(bulletCol, bossCol);

        Destroy(bullet, 2.5f);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ContactPoint2D contact = collision.GetContact(0);
            if (contact.normal.y < -0.5f)
            {
                TakeDamage(1);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health > 0)
        {
            UpdatePhaseStats();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        if (edgeCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(edgeCheck.position, edgeCheck.position + Vector3.down * edgeRayDistance);
        }
    }
}