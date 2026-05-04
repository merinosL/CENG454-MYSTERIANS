using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Nextlevel : MonoBehaviour
{
 private Scene _scene;
    private void Awake()
    {
        _scene = SceneManager.GetActiveScene();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (SceneManager.GetActiveScene().name == "game_over")
            {
                SceneManager.LoadScene(0);

            }
            else if (SceneManager.GetActiveScene().name == "finishScene")
            {
                SceneManager.LoadScene(0);

            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_scene.buildIndex >= SceneManager.sceneCount - 1)
        {
            SceneManager.LoadScene(0);
        }
        
        if(other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(_scene.buildIndex+1);
        }
    }

    public void StartLevel()
    {
        if (_scene.buildIndex >= SceneManager.sceneCount - 1)
        {
            SceneManager.LoadScene(0);
        }
        
        SceneManager.LoadScene(_scene.buildIndex+1);
    }
}
