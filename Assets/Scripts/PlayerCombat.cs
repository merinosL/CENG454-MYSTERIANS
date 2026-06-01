using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackCooldown = 0.5f;
    public float hitDelay = 0.5f;

    [Header("Hitbox Settings")]
    public Transform attackPoint;
    public float attackRange = 2f;
    public LayerMask attackableLayers;
    public LayerMask enemyLayers;

    [Header("Damage Settings")]
    public int attackDamage = 1;

    private Animator _animator;
    private bool _canAttack = true;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _canAttack)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        _canAttack = false;


        
        Debug.Log("Kýlýç sesi tetiklendi!");
        SoundManager.Instance.PlaySound2D("SwordSwing");
        _animator.SetTrigger("attack");
        yield return new WaitForSeconds(hitDelay);

        DoDamage();
        BreakDestructibles();

        yield return new WaitForSeconds(attackCooldown - hitDelay);
        _canAttack = true;
    }

    void DoDamage()
    {
        if (attackPoint == null) return;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }
    }

    void BreakDestructibles()
    {
        if (attackPoint == null) return;

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, attackableLayers);

        foreach (Collider2D obj in hitObjects)
        {
            IDestructible destructible = obj.GetComponent<IDestructible>();
            if (destructible != null) destructible.Break();

            ICollectible collectible = obj.GetComponent<ICollectible>();
            if (collectible != null) collectible.Collect();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}