using UnityEngine;

public class Medicine : MonoBehaviour
{
    [Header("Health Settings")]
    public int healAmount = 1; 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            
           
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
                Debug.Log("Health has been increased by 1!");
                
                Destroy(gameObject); 
            }
        }
    }
}