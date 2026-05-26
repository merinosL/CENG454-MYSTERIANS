using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;

    public int maxHealth = 5;
    public int currentHealth = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
