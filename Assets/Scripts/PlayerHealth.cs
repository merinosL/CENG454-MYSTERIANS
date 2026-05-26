using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public event Action<int> OnHealthChanged;
    public event Action OnDeath;

    void Start()
    {
        OnHealthChanged?.Invoke(HealthManager.Instance.currentHealth);
    }

    public void TakeDamage(int damage)
    {
        HealthManager.Instance.currentHealth -= damage;

        if (HealthManager.Instance.currentHealth < 0)
            HealthManager.Instance.currentHealth = 0;

        OnHealthChanged?.Invoke(HealthManager.Instance.currentHealth);

        if (HealthManager.Instance.currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        HealthManager.Instance.currentHealth += amount;

        if (HealthManager.Instance.currentHealth > HealthManager.Instance.maxHealth)
            HealthManager.Instance.currentHealth = HealthManager.Instance.maxHealth;

        Debug.Log("HEAL -> " + HealthManager.Instance.currentHealth);

        OnHealthChanged?.Invoke(HealthManager.Instance.currentHealth); 
    }
    void Die()
    {
        OnDeath?.Invoke();
        gameObject.SetActive(false);
    }

    public int CurrentHealth
    {
        get { return HealthManager.Instance.currentHealth; }
    }
}