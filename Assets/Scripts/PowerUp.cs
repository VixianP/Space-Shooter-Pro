using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    [SerializeField]
    private float speed = 3;

    [SerializeField]
    private int PowerUpID;

    private bool IsFollowing;
    [SerializeField]
    GameObject ObjectToFollow;

    [SerializeField]
    private AudioClip PowerUpAudio;

    TrailingEnemies TE;
    EnemyTrailHandler TH;
    Enemy EnemyScript;

    void Update()
    {
        if(ObjectToFollow == null)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        } else if(ObjectToFollow != null && IsFollowing == true)
        {
            MoveTowards();
        }

        Boundaries();
    }
    void Boundaries()
    {
        if (transform.position.y < -5.8f)
        {
            Destroy(gameObject);
        }
    }
    public void MoveTowards()
    {
        TE = ObjectToFollow.GetComponent<TrailingEnemies>();
        if (Vector2.Distance(gameObject.transform.position,ObjectToFollow.transform.position) < 1)
            {
            TE.AddCargo(gameObject);
            transform.parent = TE.gameObject.transform;
            gameObject.SetActive(false);
        } else if(Vector2.Distance(gameObject.transform.position,ObjectToFollow.transform.position) > 0)
            {
            transform.position = Vector2.Lerp(transform.position, ObjectToFollow.transform.position, speed * 2f * Time.deltaTime);
        }
        
    }
    public void SetObject(GameObject Object)
    {
        if (IsFollowing == false)
        {
            ObjectToFollow = Object;
            IsFollowing = true;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            switch (PowerUpID)
            {
                case 0:
                    player.TripleShotActive();
                    break;
                case 1:
                    player.SpeedBoost();
                    break;
                case 2:
                    player.ShieldActive();
                    break;
                case 3:
                    player.Reload();
                    break;
                case 4:
                    player.Heal();
                    break;
                case 5:
                    player.RocketActive();
                    break;
                case 6:
                    player.SpawnAssist();
                    break;
        }
            AudioSource.PlayClipAtPoint(PowerUpAudio, transform.position);
            Destroy(gameObject);
        }
        if(other.tag == "Enemy")
        {
            if(other.gameObject.GetComponent<Enemy>() != null)
            {
                EnemyScript = other.gameObject.GetComponent<Enemy>();
            }
        }
    }

}
