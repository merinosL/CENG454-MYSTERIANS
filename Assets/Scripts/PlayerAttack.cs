using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
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

    private bool facingRight = true;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleFlip();

        if (Input.GetMouseButtonDown(0) && _canAttack)
        {
            StartCoroutine(AttackRoutine());
        }

        UpdateAttackPoint();
    }

    IEnumerator AttackRoutine()
    {
        _canAttack = false;

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

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayers
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();

            if (enemyAI != null)
            {
                enemyAI.TakeDamage(attackDamage);
            }

            BossAI bossAI = enemy.GetComponent<BossAI>();

            if (bossAI != null)
            {
                bossAI.TakeDamage(attackDamage);
            }
        }
    }

    void BreakDestructibles()
    {
        if (attackPoint == null) return;

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            attackableLayers
        );

        foreach (Collider2D obj in hitObjects)
        {
            IDestructible destructible = obj.GetComponent<IDestructible>();
            if (destructible != null)
            {
                destructible.Break();
            }
        }
    }

    void HandleFlip()
    {
        float move = Input.GetAxisRaw("Horizontal");

        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void UpdateAttackPoint()
    {
        if (attackPoint == null) return;

        Vector3 pos = attackPoint.localPosition;

        pos.x = facingRight ? Mathf.Abs(pos.x) : -Mathf.Abs(pos.x);

        attackPoint.localPosition = pos;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
