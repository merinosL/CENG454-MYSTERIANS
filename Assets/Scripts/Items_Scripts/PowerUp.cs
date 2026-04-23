using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PlayerController.PlayerPowerState type; 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().ApplyPowerUp(type);
            Destroy(gameObject);
        }
    }
}