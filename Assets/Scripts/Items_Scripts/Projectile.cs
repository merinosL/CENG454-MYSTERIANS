using UnityEngine;

public class Projectile : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 5f); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject); 
            Destroy(gameObject);
        }
    }
}