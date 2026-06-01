using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSkipper : MonoBehaviour
{
    public string targetSceneName; 

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(targetSceneName);
        }
    }
}