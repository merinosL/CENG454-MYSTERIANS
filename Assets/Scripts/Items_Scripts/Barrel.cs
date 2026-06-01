using UnityEngine;

public class Barrel : MonoBehaviour, IDestructible
{
    [Header("Loot Settings")]
    public GameObject medicinePrefab;

    public void Break()
    {
        Debug.Log("Sword hit received! Barrel is breaking...");

        SoundManager.Instance.PlaySound3D("BarrelBreak", transform.position);

        if (medicinePrefab != null)
        {
            GameObject medicine = Instantiate(medicinePrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);

            var col = medicine.GetComponent<Collider2D>();
            if (col != null)
            {
                col.isTrigger = true;
            }

            Rigidbody2D rb = medicine.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(new Vector2(0f, 4f), ForceMode2D.Impulse);
            }
        }

        Destroy(gameObject);
    }
}