using UnityEngine;

public class BossAI : MonoBehaviour
{
    public bool isAwake = false;
    
    [Header("Devriye Ayarları (Uyku Modu)")]
    public float patrolRadius = 5f;
    public float patrolJumpForceX = 6f;
    public float patrolJumpForceY = 10f;
    public float patrolJumpDelay = 4f;

    [Header("Savaş Ayarları (Uyanık Mod)")]
    public float jumpForceX = 16f;
    public float jumpForceY = 18f;
    public float jumpDelay = 2.5f;

    [Header("Can Sistemi")]
    public int health = 3;
    public Color phase2Color = Color.red;

    [Header("Sensörler")]
    public Transform groundCheck;
    public Transform edgeCheck;
    public float groundCheckRadius = 0.4f;
    public float edgeRayDistance = 2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Transform player;
    private SpriteRenderer spriteRenderer;
    private float jumpTimer;
    private bool isGrounded;
    private bool facingRight = true;
    private float leftBoundary;
    private float rightBoundary;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        jumpTimer = patrolJumpDelay;

        leftBoundary = transform.position.x - patrolRadius;
        rightBoundary = transform.position.x + patrolRadius;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            jumpTimer -= Time.deltaTime;
            
            if (jumpTimer <= 0f)
            {
                if (isAwake)
                {
                    LookAtPlayer();
                    CheckAndJump(jumpForceX, jumpForceY, false);
                    jumpTimer = jumpDelay;
                }
                else
                {
                    CheckAndJump(patrolJumpForceX, patrolJumpForceY, true);
                    jumpTimer = patrolJumpDelay;
                }
            }
        }
    }

    void LookAtPlayer()
    {
        if (player == null) return;
        if (player.position.x > transform.position.x && !facingRight) Flip();
        else if (player.position.x < transform.position.x && facingRight) Flip();
    }

    void CheckAndJump(float forceX, float forceY, bool checkBoundaries)
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
            JumpAttack(forceX, forceY);
        }
        else
        {
            Flip();
        }
    }

    void JumpAttack(float forceX, float forceY)
    {
        float direction = facingRight ? 1f : -1f;
        rb.linearVelocity = Vector2.zero;
        Vector2 jumpVector = new Vector2(direction * forceX, forceY);
        rb.AddForce(jumpVector * rb.mass, ForceMode2D.Impulse);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
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

        float drawLeft = Application.isPlaying ? leftBoundary : transform.position.x - patrolRadius;
        float drawRight = Application.isPlaying ? rightBoundary : transform.position.x + patrolRadius;
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector2(drawLeft, transform.position.y - 2), new Vector2(drawLeft, transform.position.y + 2));
        Gizmos.DrawLine(new Vector2(drawRight, transform.position.y - 2), new Vector2(drawRight, transform.position.y + 2));
    }
}