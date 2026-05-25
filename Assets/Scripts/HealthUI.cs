using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Image[] hearts;

    void Update()
    {
        if (playerHealth == null) return;

        int hp = playerHealth.CurrentHealth;
        Debug.Log("HP UI: " + hp);

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < hp;
        }
    }
    
}