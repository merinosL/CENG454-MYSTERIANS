using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public int damage = 1;
    public string playerTag = "Player";
    private Animator anim;
    private Rigidbody2D rb;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        if (GetComponent<Collider2D>() != null) GetComponent<Collider2D>().enabled = true;
        if (anim != null) anim.Rebind();

        Invoke(nameof(DeactivateBullet), 3f);
    }

    void OnDisable()
    {
        CancelInvoke();
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

        CancelInvoke(nameof(DeactivateBullet));

        if (anim != null)
        {
            anim.SetTrigger("Explode");
            Invoke(nameof(DeactivateBullet), 0.5f);
        }
        else
        {
            DeactivateBullet();
        }
    }

    void DeactivateBullet()
    {
        gameObject.SetActive(false);
    }
}