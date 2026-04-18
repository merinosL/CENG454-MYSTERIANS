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
    public float rayDistance = 1f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool movingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                PatrolLogic();
                break;
            case State.Chase:
                break;
            case State.Dead:
                rb.velocity = Vector2.zero;
                break;
        }
    }

    void PatrolLogic()
    {
        rb.velocity = new Vector2((movingRight ? moveSpeed : -moveSpeed), rb.velocity.y);

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

    private void OnDrawGizmos()
    {
        if (edgeCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(edgeCheck.position, edgeCheck.position + Vector3.down * rayDistance);
        }
    }
}