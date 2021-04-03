using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    GameObject SpawnManagerGameObject;
    [SerializeField]
    private int CurrentScene;
 
    public void GameOverRestart()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(CurrentScene);
        }
    }
    
}
