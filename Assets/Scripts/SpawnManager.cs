using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private float EnemySpawnTime = 5f;
    [SerializeField]
    private GameObject Enemy;
    [SerializeField]
    private GameObject EnemyContainer;
 
    [SerializeField]
    private GameObject[] PowerUp;
    private int PowerUpSelection;

    [SerializeField]
    GameObject UIManagerGameObject;
    [SerializeField]
    GameObject SceneLoaderGameObject;
  
    private bool IsPlayerDead = false;

    UIManager UIM;
    SceneLoader SL;
    
    private void Awake()
    {
        SL = SceneLoaderGameObject.GetComponent<SceneLoader>();
        UIM = UIManagerGameObject.GetComponent<UIManager>();
        StartCoroutine(EnemyCoroutine(EnemySpawnTime));
        StartCoroutine(PowerUpCoroutine());

    }

    private void Update()
    {
        GameOverInput();
    }

    IEnumerator PowerUpCoroutine()
    {
        while (IsPlayerDead == false)
        {
            PowerUpSelection = Random.Range(0, PowerUp.Length);
            GameObject NewPowerUp = Instantiate(PowerUp[PowerUpSelection], new Vector3(Random.Range(-7.20f, 7.20f), 8f, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }
   
    IEnumerator EnemyCoroutine(float time)
    {
        while (IsPlayerDead == false)
        {
            GameObject NewEnemy = Instantiate(Enemy, new Vector3(Random.Range(-8.20f, 8.20f), 8f, 0), Quaternion.Euler(180, 0, 0));
            NewEnemy.transform.parent = EnemyContainer.transform;
            yield return new WaitForSeconds(time);
        }
    }
    public void OnPlayerDeath()
    {
        IsPlayerDead = true;
        UIM.GameOver();
    }
    public void GameOverInput()
    {
        if (IsPlayerDead == true)
        {
            SL.GameOverRestart();
        }
    }
}
