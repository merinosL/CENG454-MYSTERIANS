using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public WinLoseManager winManager;
    public bool isLevel3 = false;
    public GameObject boss;

    void Start()
    {
        if (winManager == null) winManager = FindObjectOfType<WinLoseManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isLevel3)
            {
                if (boss != null && boss.activeInHierarchy)
                {
                    Debug.Log("Boss henüz ölmedi! Çýkýþ yapýlamaz.");
                    return;
                }

                if (ScoreManager.Instance.score <= 6)
                {
                    SceneManager.LoadScene("EndScene");
                }
                else
                {
                    SceneManager.LoadScene("Outro");
                }
            }
            else
            {
                if (winManager != null) winManager.OpenWinPanel();
            }
        }
    }
}