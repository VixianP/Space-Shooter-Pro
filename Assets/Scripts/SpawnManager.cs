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
    private GameObject PowerUp;
  

    private bool IsPlayerDead = false;

    
    private void Awake()
    {
        StartCoroutine(EnemyCoroutine(EnemySpawnTime));
        StartCoroutine(PowerUpCoroutine());
    }

    IEnumerator PowerUpCoroutine()
    {

        while (IsPlayerDead == false)
        {
            GameObject NewPowerUp = Instantiate(PowerUp, new Vector3(Random.Range(-8.20f, 8.20f), 8f, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3,8));
        }
    }
    IEnumerator EnemyCoroutine(float time)
    {
      
        while (IsPlayerDead == false)
        {
           GameObject NewEnemy = Instantiate(Enemy, new Vector3(Random.Range(-8.20f, 8.20f),8f,0), Quaternion.Euler(180,0,0));
            NewEnemy.transform.parent = EnemyContainer.transform;
            yield return new WaitForSeconds(time);
        }
    }
    public void OnPlayerDeath()
    {
        IsPlayerDead = true;
    }
}
