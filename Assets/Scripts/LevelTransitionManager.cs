using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionManager : MonoBehaviour
{
    public void CheckScoreAndTransition()
    {
        int currentScore = ScoreManager.Instance.score;

        if (currentScore <= 6)
        {
            SceneManager.LoadScene("EndScene");
        }
        else
        {
            SceneManager.LoadScene("Outro");
        }
    }
}