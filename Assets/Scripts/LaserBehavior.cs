using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehavior : MonoBehaviour
{   
    [SerializeField]
    private float speed;
    [SerializeField]
    private int AttackPower;

    Enemy EnemyScript;
    Player PlayerScript;

    [SerializeField]
    bool IsPlayer;


    // Update is called once per frame
    void Update()
    {

        transform.Translate(new Vector3(0, speed, 0) * Time.deltaTime);
        if(transform.position.y > 8)
        {
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        } else if(transform.position.y < -8)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsPlayer == true)
        {
            if (collision.tag == "Enemy")
            {
                EnemyScript = collision.GetComponent<Enemy>();
                EnemyScript.EDamage(AttackPower);
                Destroy(gameObject);
            }
        }
        if (IsPlayer == false)
        {
            if (collision.tag == "Player")
            {
                PlayerScript = collision.GetComponent<Player>();
                PlayerScript.Damage(AttackPower);
                Destroy(gameObject);
            }
        }
    }
    public void UpdateSpecs(float LSpeed, int Dmg)
    {
        speed = LSpeed;
        AttackPower = Dmg;
    }
}
