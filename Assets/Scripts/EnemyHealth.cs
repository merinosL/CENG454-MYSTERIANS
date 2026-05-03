using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("Normal Düşman: 1, Ranged: 3, Boss: 5")]
    [SerializeField] private int maxHealth = 1; 
    private int currentHealth;

    public event Action<int> OnHealthChanged;
    public event Action OnDeath;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        if (currentHealth <= 0) return; 

        currentHealth -= damageAmount;
        
        OnHealthChanged?.Invoke(currentHealth);
        Debug.Log($"{gameObject.name} hasar aldı! Kalan can: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
        Debug.Log($"{gameObject.name} öldü!");
        Destroy(gameObject); 
    }
}