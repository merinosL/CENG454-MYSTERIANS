using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    public BossAI boss; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (boss != null)
            {
                boss.isAwake = true;
                Debug.Log("Entered the Boss' Room, War Starts!");
            }
            
            Destroy(gameObject); 
        }
    }
}