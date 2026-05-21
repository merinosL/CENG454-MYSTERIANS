using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        Debug.Log("<color=green>PLAYER LOG:</color> Player started with " + currentHealth + " HP.");
    }

    void OnEnable()
    {
        EnemyAI.OnPlayerContact += TakeDamage;
    }

    void OnDisable()
    {
        EnemyAI.OnPlayerContact -= TakeDamage;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("<color=red>PLAYER LOG:</color> Player took damage! Remaining HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (animator != null) animator.SetTrigger("Hurt");
        }
    }

    void Die()
    {
        Debug.Log("<color=red>PLAYER LOG:</color> Player died!");
        if (animator != null) animator.SetTrigger("Death");
        
        Collider2D coll = GetComponent<Collider2D>();
        if (coll != null) coll.enabled = false;
        
        this.enabled = false;
    }
}