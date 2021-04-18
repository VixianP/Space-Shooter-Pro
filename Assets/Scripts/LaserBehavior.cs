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

    Enemy EnemyScript;

    [SerializeField]
    bool IsHomming;
    [SerializeField]
    private float RateOfRotation;

    private GameObject[] EnemyList;
    private GameObject EnemyToFollow;

    private void Awake()
    {
        EnemyList = GameObject.FindGameObjectsWithTag("Enemy");

        if (EnemyList.Length > 0)
        {
            EnemyToFollow = EnemyList[Random.Range(0, EnemyList.Length)]; // will be assigned if null
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
        if (IsHomming == true)
        {
            if (EnemyToFollow == null)
            {
                transform.Translate(Vector3.up * speed * Time.deltaTime);
                    if (EnemyList.Length > 0)
                    {
                        EnemyToFollow = EnemyList[Random.Range(0, EnemyList.Length)];
                    }
            } else if (EnemyToFollow != null)
            {
                if (EnemyToFollow.transform.position.y > -3f)
                {
                    transform.Translate(new Vector3(0, speed, 0) * Time.deltaTime);
                    transform.RotateAround(transform.position, EnemyToFollow.transform.position, RateOfRotation); //stream shot
                } else
                {
                    EnemyToFollow = null;
                }
            }
        } else
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.tag == "Enemy")
            {
                EnemyScript = collision.GetComponent<Enemy>();
                EnemyScript.EDamage(AttackPower);
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
}
