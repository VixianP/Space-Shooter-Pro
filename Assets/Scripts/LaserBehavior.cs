using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehavior : MonoBehaviour
{   
    [SerializeField]
    private float speed;
    [SerializeField]
    private int AttackPower;
    [SerializeField]
    private float TimeToExpire;

    [SerializeField]
    bool IsHomming;
    [SerializeField]
    bool IsRocket;

    [SerializeField]
    private float RateOfRotation;

    private GameObject[] EnemyList;
    private GameObject EnemyToFollow;

    [SerializeField]
    private GameObject RocketExplosion;

    [SerializeField]
    private Vector2 RocketExplosionSize = new Vector2(2, 2);


    private void Awake()
    {
        EnemyList = GameObject.FindGameObjectsWithTag("Enemy");

        if (EnemyList.Length > 0)
        {
            EnemyToFollow = EnemyList[Random.Range(0, EnemyList.Length)]; // will be assigned if null
        }
        if(IsRocket == true)
        {
            StartCoroutine(RocketTimeDetonate());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject, TimeToExpire);
        } else
        {
            Destroy(gameObject, TimeToExpire);
        }
        Cruise();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.tag == "Enemy" && IsRocket == false)
            {
            collision.SendMessage("EDamage",AttackPower);
                Destroy(gameObject);
            } else if(collision.tag == "Enemy" && IsRocket == true)
        {
            Collider2D[] EnemiesInRange = Physics2D.OverlapCapsuleAll(transform.position, RocketExplosionSize, CapsuleDirection2D.Horizontal, 0);
            for(int i = 0; i < EnemiesInRange.Length; i++)
            {
                EnemiesInRange[i].SendMessage("EDamage", AttackPower);
            }
            Instantiate(RocketExplosion,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void UpdateSpecs(float LSpeed, int Dmg)
    {
        speed = LSpeed;
        AttackPower = Dmg;
    }
    public void ReAssign(GameObject NewEnemyPOS)
    {
        EnemyToFollow = NewEnemyPOS;
    }
    void Cruise()
    {
        if (IsHomming == true)
        {
            if (EnemyToFollow == null)
            {
                transform.Translate(Vector3.up * speed * Time.deltaTime);
                if (EnemyList.Length > 0)
                {
                    EnemyToFollow = EnemyList[Random.Range(0, EnemyList.Length)];
                }
            }
            else if (EnemyToFollow != null)
            {
                if (EnemyToFollow.transform.position.y > -3f)
                {
                    transform.Translate(new Vector3(0, speed, 0) * Time.deltaTime);
                    transform.RotateAround(transform.position, EnemyToFollow.transform.position, RateOfRotation);
                }
                else
                {
                    EnemyToFollow = null;
                }
            }
        }
        else
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
    }
    IEnumerator RocketTimeDetonate()
    {
       
        yield return new WaitForSeconds(1f);
        Collider2D[] EnemiesInRange = Physics2D.OverlapCapsuleAll(transform.position, RocketExplosionSize, CapsuleDirection2D.Horizontal, 0);
        for (int i = 0; i < EnemiesInRange.Length; i++)
        {
            EnemiesInRange[i].SendMessage("EDamage", AttackPower);
        }
        Instantiate(RocketExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
