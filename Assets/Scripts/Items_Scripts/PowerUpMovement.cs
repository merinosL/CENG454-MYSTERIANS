using UnityEngine;

public class PowerUpMovement : MonoBehaviour
{
    public float moveSpeed = 3f; 
    
    private Rigidbody2D _rb;
    private int _direction = 1; 

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        
        _rb.linearVelocity = new Vector2(_direction * moveSpeed, _rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player")) return;

        ContactPoint2D contact = collision.GetContact(0);
        
        if (Mathf.Abs(contact.normal.x) > 0.5f)
        {
            _direction *= -1; 
        }
    }
}