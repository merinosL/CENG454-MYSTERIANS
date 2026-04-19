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
    public Transform edgeCheck;
    public float rayDistance = 2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool movingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (currentState == State.Dead)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        if (currentState == State.Patrol)
        {
            PatrolLogic();
        }
    }

    void PatrolLogic()
    {
        rb.linearVelocity = new Vector2((movingRight ? moveSpeed : -moveSpeed), rb.linearVelocity.y);

        RaycastHit2D groundInfo = Physics2D.Raycast(edgeCheck.position, Vector2.down, rayDistance, groundLayer);

        if (groundInfo.collider == false)
        {
            Flip();
        }
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
    }
}