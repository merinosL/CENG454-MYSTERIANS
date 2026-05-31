using UnityEngine;
using UnityEngine.UI;

public class EnemyUIUpdater : MonoBehaviour
{
    public Image fillImage;
    private EnemyHealth enemyHealth;
    private float maxHealth;
    private float targetFillAmount;
    public float lerpSpeed = 5f;

    private void Awake()
    {
        enemyHealth = GetComponentInParent<EnemyHealth>();
    }

    private void Start()
    {
        if (enemyHealth != null)
        {
            maxHealth = (float)enemyHealth.maxHealth;
            targetFillAmount = 1f;
            fillImage.fillAmount = targetFillAmount;
        }
    }

    private void OnEnable()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnHealthChanged += UpdateTarget;
            targetFillAmount = 1f;
            fillImage.fillAmount = 1f;
        }
    }

    private void OnDisable()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnHealthChanged -= UpdateTarget;
        }
    }

    private void Update()
    {
        if (fillImage != null && Mathf.Abs(fillImage.fillAmount - targetFillAmount) > 0.001f)
        {
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFillAmount, Time.deltaTime * lerpSpeed);
        }
    }

    private void UpdateTarget(int currentHealth)
    {
        targetFillAmount = (float)currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            fillImage.fillAmount = 0f;
        }
    }
}