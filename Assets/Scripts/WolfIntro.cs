using UnityEngine;

public class WolfIntro : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Growl()
    {
        animator.SetBool("isGrowling", true);

        audioSource.Play();
    }
}
