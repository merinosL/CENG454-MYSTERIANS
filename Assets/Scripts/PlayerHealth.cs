using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;


    public event Action<int> OnHealthChanged;
    public event Action OnDeath;

    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth); 
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        
        OnHealthChanged?.Invoke(currentHealth);
        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (currentHealth >= maxHealth) return; 

        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth; 

        OnHealthChanged?.Invoke(currentHealth);
        Debug.Log("The Player Has Recovered! New Life:" + currentHealth);
    }

    void Die()
    {
        Debug.Log("Player Died");
        OnDeath?.Invoke(); 
        gameObject.SetActive(false);
    }
}