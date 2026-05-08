using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackCooldown = 0.5f;
    
    [Header("Hitbox Settings")]
    public Transform attackPoint; 
    public float attackRange = 0.5f; 
    public LayerMask attackableLayers; 

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

        _animator.SetTrigger("attack");

        float hitDelay = 0.5f; 
        yield return new WaitForSeconds(hitDelay);
        
        if (attackPoint != null)
        {
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, attackableLayers);

            foreach (Collider2D obj in hitObjects)
            {
                IDestructible destructible = obj.GetComponent<IDestructible>();
                if (destructible != null)
                {
                    destructible.Break();
                }
            }
        }
        yield return new WaitForSeconds(attackCooldown - hitDelay);

        _canAttack = true;
    }
    
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}