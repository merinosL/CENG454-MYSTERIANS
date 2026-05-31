using UnityEngine;

public class SpeedPotion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player =
                other.GetComponent<PlayerController>();

            if (player != null)
            {
                player.StartCoroutine(
                    player.SpeedBoost(2f, 3f));
            }

            Destroy(gameObject);
        }
    }
}