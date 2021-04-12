using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
     private int EnemyCollisionDamage = 1;
    [SerializeField]
    private int PointValue = 5;
    [SerializeField]
    float EnemyFireRate = 1;
    [SerializeField]
    float TrailRate;
    [SerializeField]
    private int EnemyHealth;
    [SerializeField]
    private int LaserSpeed;
    [SerializeField]
    private int LaserDamage;

    Player player;
    LaserBehavior ElaserSpecs;

    Animator EnemyAnimator;
    Collider2D EnemyCollider;

    [SerializeField]
    AudioClip[] EnemyAudioClips;

    [SerializeField]
    private GameObject Elaser;

    private bool IsDead;
    // Start is called before the first frame update
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
    void Emovement()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
    void Eboundaries()
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
                EnemyCollider.enabled = false;
                speed = 1;
                EnemyAnimator.SetTrigger("ED");
                AudioSource.PlayClipAtPoint(EnemyAudioClips[1], transform.position);
                Destroy(gameObject);
        }
    }
    void EnemyFire()
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
    public void EDamage(int Dmg)
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
            Destroy(gameObject);
        }
    }
}
