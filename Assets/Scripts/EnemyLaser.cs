using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private int AttackPower;
    [SerializeField]
    private float TimeToExpire;

    Enemy EnemyScript;
    Player PlayerScript;


    void Update()
    {
        transform.Translate(0, speed * Time.deltaTime, 0);
        Destroy(gameObject, TimeToExpire);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerScript = collision.GetComponent<Player>();
            if (collision.tag == "Player")
            {
                PlayerScript.Damage(AttackPower,gameObject);
                
            }
        
        
    }
    public void UpdateSpecs(float LSpeed, int Dmg)
    {
        speed = LSpeed;
        AttackPower = Dmg;
    }
}
