using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailingEnemies : MonoBehaviour
{
    //trail enemies will absorb power ups and can be reaquired by killing them. Additionally, they will drop their own power ups. These guys are cargo carriers

    public List<GameObject> Cargo = new List<GameObject>();
    [SerializeField]
    private int damage;
    [SerializeField]
    private int PointValue;
    [SerializeField]
    private float TEHealth;
    [SerializeField]
    private float TEMovementSpeed;

    [SerializeField]
    AudioClip[] TEAudioClip;
    [SerializeField]
    private GameObject TargetToFollow;
    [SerializeField]


    GameObject player;
    Animator TEAnimator;
    Collider2D TECollider;
    SpriteRenderer TeRenderer;

    public EnemyTrailHandler ETH;
    Player PlayerScript;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
            PlayerScript = player.GetComponent<Player>();
        }
        TEAnimator = GetComponent<Animator>();
        TECollider = GetComponent<Collider2D>();
        TeRenderer = GetComponent<SpriteRenderer>();
        //ETH = GameObject.Find("TrailingEnemyHandler").GetComponent<EnemyTrailHandler>();
    }

    void Update()
    {
       ETrailMovement();
       DeathBounds();
    }

    void ETrailMovement()
    {
            if (TargetToFollow != null)
            {
                if (Vector2.Distance(transform.position, TargetToFollow.transform.position) > 1.1f)
                {
                    transform.position = Vector2.Lerp(transform.position, TargetToFollow.transform.position, TEMovementSpeed * Time.deltaTime);
                }
            } else if(TargetToFollow == null)
        {
            transform.Translate(0, -TEMovementSpeed * Time.deltaTime, 0);
        }
            
    }
    public void ReTarget(GameObject NewTarget)
    {
        TargetToFollow = NewTarget;
    } 

    public  void TrailDamage(int Dmg)
    {
        TEHealth -= Dmg;
        if (TEHealth < 1)
        {
            TECollider.enabled = false;
            TEAnimator.SetTrigger("ED");
            PlayerScript.AddPoints(PointValue);
            AudioSource.PlayClipAtPoint(TEAudioClip[1], transform.position);
            TeRenderer.enabled = false;
            ETH.EtrailDeath(gameObject);
            Destroy(gameObject, 2);
        }
    }
     public void DeathBounds()
    {
        if (transform.position.y < -11)
        {
            if (ETH != null)
            {
                ETH.EtrailDeath(gameObject);
                Destroy(gameObject, 1);
            }
            else
            {
                Destroy(gameObject, 1);
            }
        }
    }

    //pull n store powerup / hazard function. if true, itll be constantly called in update to move object towards itself

    private  void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            
            if (PlayerScript.IsInvul == false && PlayerScript.IsDodging == false)
            {
                PlayerScript = other.GetComponent<Player>();
                PlayerScript.Damage(damage);
                TrailDamage(damage);
            }
        }
        if(other.tag == "Laser")
        {
            TrailDamage(1);
        }
    }

}
