using UnityEngine;

public class IntroWalk : MonoBehaviour
{
    public WolfIntro wolf;
    public float moveSpeed = 2f;
    public float walkTime = 4f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        animator.SetBool("isRunning", true);

        Invoke("StopWalking", walkTime);
    }

    void Update()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }

    void StopWalking()
    {
        wolf.Growl();
        moveSpeed = 0f;

        animator.SetBool("isRunning", false);
    }
}
