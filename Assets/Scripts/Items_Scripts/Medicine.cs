using UnityEngine;

public class Medicine : MonoBehaviour
{
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("İlaç toplandı!");
            
        
            Destroy(gameObject); 
        }
    }
}