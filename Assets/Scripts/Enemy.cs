using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
     private int EnemyDamage = 1;
    [SerializeField]
    private int PointValue = 5;

    private bool IsDead;

    Player player;
    Animator EnemyAnimator;
    Collider2D EnemyCollider;
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
        speed = Random.Range(2, 9);
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
        
        if(other.tag == "Laser")
        {
            EnemyCollider.enabled = false;
            EnemyAnimator.SetTrigger("ED");
            player.AddPoints(PointValue);
            speed = 1;
            Destroy(other.gameObject);
            Destroy(gameObject, 0.6f);

        }
        if(other.tag == "Player")
        {
                EnemyCollider.enabled = false;
                speed = 1;
                EnemyAnimator.SetTrigger("ED");
                player.Damage(EnemyDamage);
                IsDead = true;
                Destroy(gameObject,0.6f);
        }
    }
}
