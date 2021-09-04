using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrailHandler : MonoBehaviour
{

    //if trail is bigger than 5, then enemies drop more loot and HAZARDS

    [SerializeField]
    public List<GameObject> EnemiesInTrail = new List<GameObject>();
    int MaxTrailSize;
    private int TrailLeftToSpawn;
    //random trail size per spawn

    private float TurnTimer = -1f;
    private Vector2 Direction;

    [SerializeField]
    private GameObject EnemyToSpawn;
    private GameObject SpawnPos;
    [SerializeField]
    private float Speed;

    private bool BoundsHit;

    TrailingEnemies TE;
    SpawnManager SM;

    void Start()
    {
        SM = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        StartCoroutine(ETS());
        SpawnPos = gameObject;
        MaxTrailSize = Random.Range(5, 8);
        TrailLeftToSpawn = MaxTrailSize;
    }

    private void Update()
    {
        LeftRightBounds();
        LeadMovement();
        if (transform.position.y < -20)
        {
            OutOfBounds();
        }
    }

    void LeadMovement()
    {
        if (BoundsHit == false)
        {
            //enter vertically then change direction?
            if (TurnTimer < Time.time)
            {
                int RandomTurn = Random.Range(0, 2);
                switch (RandomTurn)
                {
                    case 0:
                        print(0);
                        Direction = Vector2.down * Speed;
                        TurnTimer = Time.time + 4;
                        break;
                    case 1:
                        print(1);
                        Direction = Vector2.left * Speed;
                        TurnTimer = Time.time + 4;
                        break;
                    case 2:
                        print(2);
                        Direction = Vector2.right * Speed;
                        TurnTimer = Time.time + 4;
                        break;
                }
            }
        }
        transform.Translate(Direction * Time.deltaTime);
    }

    void LeftRightBounds()
    {
        if (transform.position.x > 8)
        {
            Direction = Vector2.down * Speed;
        }
        else if (transform.position.x < -8)
        {
            Direction = Vector2.down * Speed;
        }
    }

    public void EtrailDeath(GameObject TrailEnemy)
    {
        if (EnemiesInTrail.Count > 0)
        {
            int PositionInList = EnemiesInTrail.IndexOf(TrailEnemy);
            EnemiesInTrail.Remove(EnemiesInTrail[PositionInList]);
        }
        if (EnemiesInTrail.Count < 1 && TrailLeftToSpawn == 0)
        {
            SM.OnEnemyDeath(gameObject);
            Destroy(gameObject);
        }
        
    }
    void OutOfBounds()
    {
        if (TrailLeftToSpawn == 0)
        {
            if (EnemiesInTrail.Count < 1)
            {
                SM.OnEnemyDeath(gameObject);
                Destroy(gameObject, 2);
            }
        }
    }

    IEnumerator ETS()
    {
        yield return new WaitForSeconds(0.4f);
        while (EnemiesInTrail.Count < MaxTrailSize)
        {
            int TrailCount = EnemiesInTrail.Count;
            GameObject NewEnemy = Instantiate(EnemyToSpawn, SpawnPos.transform.position * 1.3f, Quaternion.identity);
            EnemiesInTrail.Add(NewEnemy);
            TE = NewEnemy.GetComponent<TrailingEnemies>();
            TE.ETH = this.gameObject.GetComponent<EnemyTrailHandler>();
            TrailLeftToSpawn--;
            if (TrailCount == 0)
            {
                TE.ReTarget(this.gameObject); 
                SpawnPos = NewEnemy;
            }
            else if (TrailCount > 0)
            {
                TE.ReTarget(EnemiesInTrail[TrailCount - 1]);
            }
            yield return new WaitForSeconds(0.4f);
        }
    }

    //function that select ship to store nearby Power Up / Hazard
 
}
