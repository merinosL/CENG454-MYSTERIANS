using UnityEngine;

public class Barrel : MonoBehaviour, IDestructible
{
    [Header("Ganimet Ayarları")]
    public GameObject medicinePrefab; 

    public void Break()
    {
        Debug.Log("Kılıç darbesi geldi! Fıçı parçalanıyor...");
        
       
        if (medicinePrefab != null)
        {
            
            GameObject medicine = Instantiate(medicinePrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            
            
            Rigidbody2D rb = medicine.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(new Vector2(0f, 4f), ForceMode2D.Impulse);
            }
        }

        
        Destroy(gameObject);
    }
}