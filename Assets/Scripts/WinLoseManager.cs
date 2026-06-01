using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WinLoseManager : MonoBehaviour
{
    public TextMeshProUGUI winScoreText;
    public GameObject winPanel;
    public GameObject losePanel;
    public string mainMenuSceneName = "MainMenu";

    void Start()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }

    public void OpenWinPanel()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            int finalScore = ScoreManager.Instance.score;
            winScoreText.text = "Score: " + finalScore.ToString();
            Time.timeScale = 0f;
        }
    }

    public void OpenLosePanel()
    {
        if (losePanel != null)
        {
            losePanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene("Outro");
        }
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;

        if (HealthManager.Instance != null)
        {
            HealthManager.Instance.currentHealth = 3;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}