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
    private GameObject TripleShot;
    [SerializeField]
    private GameObject SpeedBoost;
  

    private bool IsPlayerDead = false;

    
    private void Awake()
    {
        while (IsPlayerDead == false)
        {
            StartCoroutine(EnemyCoroutine(EnemySpawnTime));
            StartCoroutine(TripleShotCoroutine());
            StartCoroutine(SpeedBoostCoroutine());
        }
    }

    IEnumerator SpeedBoostCoroutine()
    {

            GameObject NewPowerUp = Instantiate(SpeedBoost, new Vector3(Random.Range(-8.20f, 8.20f), 8f, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
    }
    IEnumerator TripleShotCoroutine()
    {

            GameObject NewPowerUp = Instantiate(TripleShot, new Vector3(Random.Range(-8.20f, 8.20f), 8f, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3,8));
    }
    IEnumerator EnemyCoroutine(float time)
    {
      
           GameObject NewEnemy = Instantiate(Enemy, new Vector3(Random.Range(-8.20f, 8.20f),8f,0), Quaternion.Euler(180,0,0));
            NewEnemy.transform.parent = EnemyContainer.transform;
            yield return new WaitForSeconds(time);
    }
    public void OnPlayerDeath()
    {
        IsPlayerDead = true;
    }
}
