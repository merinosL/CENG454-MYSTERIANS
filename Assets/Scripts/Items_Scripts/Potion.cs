using UnityEngine;

public class Potion : MonoBehaviour, ICollectible
{
    public void Collect()
    {
       
        SoundManager.Instance.PlaySound3D("PotionPickup", transform.position);

        PlayerInventory inventory = FindFirstObjectByType<PlayerInventory>();

        if (inventory != null)
        {
            inventory.AddPotion(1);
        }

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(1);
        }

        Destroy(gameObject);
    }
}