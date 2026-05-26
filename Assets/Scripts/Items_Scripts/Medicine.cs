using UnityEngine;

public class Medicine : MonoBehaviour
{
    public int healAmount = 1;

    private bool used = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (used) return;

        if (collision.CompareTag("Player"))
        {
            used = true;

            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);

                Debug.Log("HEAL +1");

                Destroy(gameObject);
            }
        }
    }
}