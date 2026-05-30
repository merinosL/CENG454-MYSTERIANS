using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public int damage = 1;
    public string playerTag = "Player";
    private Animator anim;
    private Rigidbody2D rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            
            Explode();
        }
        else if (collision.CompareTag("Ground") || collision.CompareTag("Wall"))
        {
            Explode();
        }
    }

    void Explode()
    {
        if (rb != null) rb.linearVelocity = Vector2.zero; 
        
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        if (anim != null)
        {
            anim.SetTrigger("Explode");
            Destroy(gameObject, 0.5f); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
}