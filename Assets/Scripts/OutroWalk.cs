using UnityEngine;
using UnityEngine.SceneManagement;

public class OutroWalk : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float walkTime = 4f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isRunning", true);
        Invoke("FinishOutro", walkTime);
    }

    void Update()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }

    void FinishOutro()
    {
        moveSpeed = 0f;
        animator.SetBool("isRunning", false);

    }
}