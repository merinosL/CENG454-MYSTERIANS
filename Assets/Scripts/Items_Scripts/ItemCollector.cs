using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Object successfully collected!");
            Destroy(gameObject); 
        }
    }
}