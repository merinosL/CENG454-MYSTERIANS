using System;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 1;
    private int currentHealth;

    public event Action<int> OnHealthChanged;
    public event Action OnDeath;

    private Rigidbody2D _rb;
    private Animator _animator;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        SetAIEnabled(true);
        StartCoroutine(SendInitialHealth());
    }

    private IEnumerator SendInitialHealth()
    {
        yield return null;
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(int damageAmount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damageAmount;
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
        SetAIEnabled(false);

        if (_animator != null)
        {
            _animator.SetTrigger("Death");
            SoundManager.Instance.PlaySound3D("EnemyDeath", transform.position);
        }

        if (_rb != null)
        {
            _rb.linearVelocity = Vector2.zero;
        }

        yield return new WaitForSeconds(1.0f);

        EnemyPooler pooler = FindAnyObjectByType<EnemyPooler>();
        if (pooler != null)
        {
            pooler.ReturnToPool(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetAIEnabled(bool state)
    {
        var enemyAI = GetComponent<EnemyAI>();
        if (enemyAI != null) enemyAI.enabled = state;

        var bossAI = GetComponent<BossAI>();
        if (bossAI != null) bossAI.enabled = state;

        var rangedAI = GetComponent<RangedEnemyAI>();
        if (rangedAI != null) rangedAI.enabled = state;

        var gatekeeperAI = GetComponent<GatekeeperAI>();
        if (gatekeeperAI != null) gatekeeperAI.enabled = state;
    }
}