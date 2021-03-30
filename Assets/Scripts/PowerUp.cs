using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    [SerializeField]
    private float speed = 3;
    [SerializeField]
    private int PowerUpID;

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
                    //speed (max to 20)
                    break;
                case 2:
                    //shield
                    break;
        }

        }
       
    }

}
