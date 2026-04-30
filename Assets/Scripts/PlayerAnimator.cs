using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public PlayerController playerController;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        animator.SetBool("isGrounded", playerController.isGrounded);
        animator.SetFloat("speed", Mathf.Abs(rb.linearVelocity.x));
    }
}