using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackCooldown = 0.5f;

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

        yield return new WaitForSeconds(attackCooldown);

        _canAttack = true;
    }
}
