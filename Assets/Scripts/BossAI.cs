using UnityEngine;

public class BossAI : MonoBehaviour
{
    public float jumpForceX = 12f;
    public float jumpForceY = 14f;
    public float jumpDelay = 3f;
    public Transform groundCheck;
    public Transform edgeCheck;
    public float groundCheckRadius = 0.4f;
    public float edgeRayDistance = 2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Transform player;
    private float jumpTimer;
    private bool isGrounded;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpTimer = jumpDelay;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            LookAtPlayer();

            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0f)
            {
                CheckAndJump();
                jumpTimer = jumpDelay;
            }
        }
    }

    void LookAtPlayer()
    {
        if (player.position.x > transform.position.x && !facingRight) Flip();
        else if (player.position.x < transform.position.x && facingRight) Flip();
    }

    void CheckAndJump()
    {
        RaycastHit2D edgeInfo = Physics2D.Raycast(edgeCheck.position, Vector2.down, edgeRayDistance, groundLayer);

        if (edgeInfo.collider == true)
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
        rb.linearVelocity = new Vector2(0, 0);
        Vector2 jumpVector = new Vector2(direction * jumpForceX, jumpForceY);
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
    }
}