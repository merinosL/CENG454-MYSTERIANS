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

    public float moveSpeed = 2f;
    public float chaseSpeed = 3.5f;
    public Transform edgeCheck;
    public float rayDistance = 2f;
    public float visionRange = 5f;
    public float patrolRadius = 5f;
    public LayerMask groundLayer;
    public LayerMask playerLayer;

    private Rigidbody2D rb;
    private bool movingRight = true;
    public Transform player;

    private float leftBoundary;
    private float rightBoundary;

    void Start()
    {
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
        }
        else if (currentState == State.Patrol)
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
            ContactPoint2D contact = collision.GetContact(0);
            
            if (contact.normal.y < -0.5f)
            {
                currentState = State.Dead;
                transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.z);
                GetComponent<Collider2D>().enabled = false;
            }
            else
            {
                Debug.Log("Player Took Damage!");
            }
        }
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