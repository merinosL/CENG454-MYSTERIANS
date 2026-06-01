using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMusicHandler : MonoBehaviour
{
    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        switch (sceneName)
        {
            case "MainMenu":
            case "Level_1":
            case "Level_2":
                MusicManager.Instance.PlayMusic("MainMusic", 1.5f);
                break;
            case "Level_3":
                MusicManager.Instance.PlayMusic("BossFight", 1.5f);
                break;
            case "IntroScene":
            case "EndScene":
            case "Outro":
                MusicManager.Instance.PlayMusic("WolfStory", 1.5f);
                break;
        }
    }
}