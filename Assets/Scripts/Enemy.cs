﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;
    [SerializeField]
     private int EnemyDamage = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Random.Range(-8.30f, 8.30f), 9, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Emovement();
        Eboundaries();
    }
    void Emovement()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
    void Eboundaries()
    {
        if(transform.position.y < -5.8)
        {
            transform.position = new Vector3(Random.Range(-8.30f, 8.30f), 9, 0);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        if(other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if(player != null)
            {
                player.Damage(EnemyDamage);
                Destroy(gameObject);
            }
        }
    }
}