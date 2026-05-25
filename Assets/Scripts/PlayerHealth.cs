using UnityEngine;
using System;
using UnityEditor.PackageManager;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;
    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    public event Action<int> OnHealthChanged;
    public event Action OnDeath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

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

        if (currentHealth <= 0)
            Die();
    }
    public void Heal(int amount)
    {
        currentHealth += amount;

        Debug.Log("HEAL CALLED -> " + currentHealth);
    }




    void Die()
    {
        OnDeath?.Invoke();
        gameObject.SetActive(false);
    }
}