using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //enemy specs
    [SerializeField]
    private float EnemySpawnTime = 5f;
    [SerializeField]
    private GameObject[] Enemy;
    [SerializeField]
    private GameObject EnemyContainer;
    [SerializeField]
    private int MaxEnemies;
    private int Randomizer = 0;
    [SerializeField]
    private List<GameObject> EnemiesOnScreen = new List<GameObject>();

    //Timers
    [SerializeField]
    private float TimeInBettweenWavesTimer;
    private bool AllDead;

    //Wave Control
    [SerializeField]
    private int EnemyCount;
    private int AmountOfEnemiesCurrentlySpawned;
    [SerializeField]
    private int MaxWave; //the max waves in a game
    [SerializeField] 
    private int CurrentWave; //the current wave

    //power up specs
    [SerializeField]
    private GameObject[] PowerUp;
    private int PowerUpSelection;
    [SerializeField]
    float Tier1PowerUpTimer = 10;


    //Other Scripts
    [SerializeField]
    GameObject UIManagerGameObject;
    [SerializeField]
    GameObject SceneLoaderGameObject;
    UIManager UIM;
    SceneLoader SL;
    Enemy EnemyScript;

    //player specs
    private bool IsPlayerDead = false;

    private void Awake()
    {
        SL = SceneLoaderGameObject.GetComponent<SceneLoader>();
        UIM = UIManagerGameObject.GetComponent<UIManager>();
        EnemyCount = MaxEnemies;
    }

    private void Update()
    {
        GameOverInput();
       //WaveControl();
    }

    IEnumerator PowerUpCoroutine()
    {
        while (IsPlayerDead == false)
        {
                if(Time.time > Tier1PowerUpTimer)
            {
                PowerUpSelection = Random.Range(3, PowerUp.Length);
                GameObject Tier1PowerUp = Instantiate(PowerUp[PowerUpSelection], new Vector3(Random.Range(-7.20f, 7.20f), 8f, 0), Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(3, 8));
                Tier1PowerUpTimer = Time.time + 10;
            }
                PowerUpSelection = Random.Range(0, 2);
                GameObject NewPowerUp = Instantiate(PowerUp[PowerUpSelection], new Vector3(Random.Range(-7.20f, 7.20f), 8f, 0), Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(EnemyCoroutine(EnemySpawnTime));
       // StartCoroutine(PowerUpCoroutine());
    }
   
    IEnumerator EnemyCoroutine(float time) //pass another parameter for randomness in Enemy Array
    {
        yield return new WaitForSeconds(1.5f);
        if (IsPlayerDead == false)
        {
            for (int i = 0; i < MaxEnemies; i++)
            {
                GameObject NewEnemy = Instantiate(Enemy[Random.Range(0,Randomizer)], new Vector3(Random.Range(-4.20f, 4.20f), 10f, 0), Quaternion.identity);
                EnemyScript = NewEnemy.GetComponent<Enemy>();
                NewEnemy.transform.parent = EnemyContainer.transform;
                EnemiesOnScreen.Add(NewEnemy);
                AmountOfEnemiesCurrentlySpawned++;
                yield return new WaitForSeconds(time);
            }
        }
    }
    public void WaveControl()
    {
        EnemyCount--;
        if (CurrentWave < MaxWave)
        {
            if (AmountOfEnemiesCurrentlySpawned >= MaxEnemies)
            {
                if (EnemyCount < 1)
                {
                    CurrentWave++;
                    MaxEnemies += 2;
                    EnemyCount = MaxEnemies;
                    EnemiesOnScreen.Clear();
                    StartCoroutine(TimeInBettweenWaves());
                    //intiate pause and wave transition animation
                }
            }
        } else
        {
            //call boss
        }
    }
    IEnumerator TimeInBettweenWaves()
    {
        yield return new WaitForSeconds(TimeInBettweenWavesTimer);
        EnemyCount = MaxEnemies;
        Randomizer = Enemy.Length;
        StartCoroutine(EnemyCoroutine(2));
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
