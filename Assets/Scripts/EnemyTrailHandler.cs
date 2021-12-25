using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrailHandler : MonoBehaviour
{
    [SerializeField]
    private int PointValue;
    //if trail is bigger than 5, then enemies drop more loot and HAZARDS

    public List<GameObject> EnemiesInTrail = new List<GameObject>(); //used to pass the power ups OR negative power ups to other ships
    [SerializeField]
    List<GameObject> PowerUPs = new List<GameObject>();
    GameObject ObjectInContainer;

    public int MaxTrailSize;
    [SerializeField]
    private int TrailLeftToSpawn;
    private int CurrentNumberInTrail;

    private float TurnTimer = -1f;
    private Vector2 Direction;

    [SerializeField]
    private GameObject EnemyToSpawn;
    private GameObject SpawnPos;
    [SerializeField]
    private float Speed;

    private bool BoundsHit;

    public bool CallOutOfBounds = false;

    TrailingEnemies TE;
    SpawnManager SM;
    Player PlayerScript;
    PowerUp PU;

    void Start()
    {
        SM = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        StartCoroutine(ETS());
        SpawnPos = gameObject;
        MaxTrailSize = Random.Range(3, 5);
        TrailLeftToSpawn = MaxTrailSize;
        PlayerScript = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        AssignPowerUp();
        LeftRightBounds();
        LeadMovement();
        if (transform.position.y < -20)
        {
            if (CallOutOfBounds == false)
            {
                OutOfBounds();
            }
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
                        Direction = Vector2.down * Speed;
                        TurnTimer = Time.time + 4;
                        break;
                    case 1:
                        Direction = Vector2.left * Speed;
                        TurnTimer = Time.time + 4;
                        break;
                    case 2:
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
        if (transform.position.x > Random.Range(5,7))
        {
            Direction = Vector2.down * Speed;
        }
        else if (transform.position.x < Random.Range(-5,-7))
        {
            Direction = Vector2.down * Speed;
        }
    }

    public void EtrailDeath()
    {
        CurrentNumberInTrail--;
        PlayerScript.AddPoints(PointValue / 2);
        if (TrailLeftToSpawn < 1 && CurrentNumberInTrail <= 0)
        {
            Instantiate(PowerUPs[Random.Range(0, PowerUPs.Count)], EnemiesInTrail[MaxTrailSize - 1].transform.position, Quaternion.identity);
            CallOutOfBounds = true;
            SM.WaveControl();
            PlayerScript.AddPoints(PointValue);
            Destroy(gameObject,2);
        }
        
    }
    void OutOfBounds()
    {
        Speed *= 2;
        if (TrailLeftToSpawn == 0)
        {
            foreach (GameObject CargoShip in EnemiesInTrail)
            {
                if (CargoShip != null)
                {
                    Destroy(CargoShip);
                }
            }
            SM.WaveControl();
            Destroy(gameObject, 2);
                CallOutOfBounds = true;
        } else
        {
            foreach(GameObject CargoShip in EnemiesInTrail)
            {
                if(CargoShip != null)
                {
                    Destroy(CargoShip);
                }
            }
            SM.WaveControl();
            Destroy(gameObject, 2);
            CallOutOfBounds = true;
        }
    }
    public void AssignPowerUp()
    {
        if (TrailLeftToSpawn <= 0)
        {
             if (ObjectInContainer != null)
            {
                if (ObjectInContainer.GetComponent<PowerUp>() != null)
                {
                    PU = ObjectInContainer.GetComponent<PowerUp>();
                    PU.SetObject(EnemiesInTrail[Random.Range(0, MaxTrailSize)]);
                }
            }
        }
    }

    IEnumerator ETS()
    {
        yield return new WaitForSeconds(0.2f);
        while (EnemiesInTrail.Count < MaxTrailSize)
        {
            int TrailCount = EnemiesInTrail.Count;
            GameObject NewEnemy = Instantiate(EnemyToSpawn, SpawnPos.transform.position * 1.3f, Quaternion.identity);
            EnemiesInTrail.Add(NewEnemy);
            TE = NewEnemy.GetComponent<TrailingEnemies>();
            TE.ETH = this.gameObject.GetComponent<EnemyTrailHandler>();
            TrailLeftToSpawn--;
            CurrentNumberInTrail++;
            if (TrailCount == 0)
            {
                TE.ReTarget(this.gameObject); 
                SpawnPos = NewEnemy;
            }
            else if (TrailCount > 0)
            {
                TE.ReTarget(EnemiesInTrail[TrailCount - 1]);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "PowerUp")
        {
            ObjectInContainer = coll.gameObject;
        }
    }
}
