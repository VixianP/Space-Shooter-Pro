using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy : MonoBehaviour
{

    public float speed;

    public int EnemyCollisionDamage = 1;

    public int PointValue = 5;

    public float EnemyFireRate = 1;

    public float TrailRate;
    
    public int EnemyHealth;
    
    public int LaserSpeed;
    
    public int LaserDamage;

    private Player player;
    private SpawnManager SM;
    private LaserBehavior ElaserSpecs;

    private Animator EnemyAnimator;
    private Collider2D EnemyCollider;


    public AudioClip[] EnemyAudioClips;


    public GameObject Elaser;

    public bool IsDead;
    //private bool IsTargeted;

    public int EnemySpawnID; //used to remove from the spawn manager list
    
    void Start()
    {
        if (player == null)
        {
            GameObject FindPlayer = GameObject.Find("Player");
            if(FindPlayer != null)
            {
                player = FindPlayer.GetComponent<Player>();
            } else
            {
                Debug.LogError("Enemy:Player is null");
            }
        }
        EnemyAnimator = GetComponent<Animator>();
        EnemyCollider = GetComponent<Collider2D>();
        ElaserSpecs = Elaser.GetComponent<LaserBehavior>();
        SM = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>() ;
        speed = Random.Range(1, 5);
        transform.position = new Vector3(Random.Range(-8.30f, 8.30f), 9, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Emovement();
        Eboundaries();
        EnemyFire();
    }
    public void Emovement()
    {

        transform.Translate(0, -speed * Time.deltaTime, 0);

    }
    public void Eboundaries()
    {
        if(transform.position.y < -5.8)
        {
            transform.position = new Vector3(Random.Range(-7.30f, 7.30f), 9, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if(player.IsInvul == false && player.IsDodging == false)
            {
                EnemyCollider.enabled = false;
                speed = 1;
                EnemyAnimator.SetTrigger("ED");
                player.Damage(EnemyCollisionDamage);
                EDamage(1);
                Destroy(gameObject);
            }
                AudioSource.PlayClipAtPoint(EnemyAudioClips[1], transform.position);
        }
        if(other.tag == "Laser")
        {
            EDamage(other.GetComponent<LaserBehavior>().AttackPower);
        }
    }
    public void EnemyFire()
    {
        if (IsDead == false && transform.position.y > 0)
        {
                if (Time.time > EnemyFireRate)
                {

                Instantiate(Elaser, transform.position, Quaternion.identity);
                EnemyFireRate = Time.time + Random.Range(0.3f, 2);
                }
        }
    }
    public virtual void EDamage(int Dmg)
    {
        EnemyHealth -= Dmg;
        if (EnemyHealth < 1)
        {
            EnemyCollider.enabled = false;
            EnemyAnimator.SetTrigger("ED");
            player.AddPoints(PointValue);
            speed = 1;
            AudioSource.PlayClipAtPoint(EnemyAudioClips[1], transform.position);
            IsDead = true;
            Destroy(gameObject,2);
            SM.OnEnemyDeath(gameObject);
        }
    }
}
