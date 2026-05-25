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
            if (PlayerHealth.Instance != null)
            {
                used = true;

                PlayerHealth.Instance.Heal(healAmount);
                Debug.Log("HEAL +1");

                Destroy(gameObject);
            }
        }
    }
}