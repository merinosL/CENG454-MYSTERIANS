using UnityEngine;

using UnityEngine.SceneManagement;
 
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

            SceneManager.sceneLoaded += OnSceneLoaded;

        }

        else

        {

            Destroy(gameObject);

        }

    }
 
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)

    {

        currentHealth = 3;

    }
 
    private void OnDestroy()

    {

        SceneManager.sceneLoaded -= OnSceneLoaded;

    }

}
 