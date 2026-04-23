using UnityEngine;

public class HeavyEnemyAI : MonoBehaviour
{
    [Header("Can Ayarları")]
    public int health = 3;

    [Header("Hareket Ayarları")]
    public float moveSpeed = 1.0f; // Diğer düşmanlardan (2f) daha yavaş
    public float patrolRadius = 4f;

    [Header("Sensörler")]
    public Transform edgeCheck;
    public float rayDistance = 2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool movingRight = true;
    private float leftBoundary;
    private float rightBoundary;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        CalculatePatrolArea();
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
            ContactPoint2D contact = collision.GetContact(0);
            
            // Eğer oyuncu kafaya zıplarsa
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
            Debug.Log("Ağır Düşman Hasar Aldı! Kalan Can: " + health);
        }
        else
        {
            Debug.Log("Ağır Düşman Yok Edildi!");
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