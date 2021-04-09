using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private int CurrentScene;
 
    public void GameOverRestart()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(CurrentScene);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(1);
        }
    }
    public void SceneToLoad(int SceneNumber)
    {
        SceneManager.LoadScene(SceneNumber);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
