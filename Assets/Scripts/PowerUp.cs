using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    [SerializeField]
    private float speed = 3;
    /*
     * 
     */
    [SerializeField]
    private int PowerUpID;

    [SerializeField]
    private AudioClip PowerUpAudio;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        Boundaries();
    }
    void Boundaries()
    {
        if (transform.position.y < -5.8f)
        {
            Destroy(gameObject);
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
       
    }


}
