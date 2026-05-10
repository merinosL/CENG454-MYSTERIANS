using System;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public static event Action<int> OnBulletHit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("<color=red>PROJECTILE HIT:</color> Player detected. Dispatching damage signal.");
            
            OnBulletHit?.Invoke(1);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground"))
        {
            Debug.Log("Projectile hit ground and was destroyed.");
            Destroy(gameObject);
        }
    }
}