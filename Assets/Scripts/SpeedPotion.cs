using UnityEngine;

public class SpeedPotion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySound3D("Powerup", transform.position);

            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                player.StartCoroutine(player.SpeedBoost(2f, 3f));
            }

            Destroy(gameObject);
        }
    }
}