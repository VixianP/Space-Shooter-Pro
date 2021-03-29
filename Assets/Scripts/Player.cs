using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float Speed = 5;
    [SerializeField]
    private GameObject LaserPrefab;
    [SerializeField]
    private GameObject TripleShot;
    [SerializeField]
    private float BulletOffSet;

    [SerializeField]
    private float CanFire = -1;
    [SerializeField]
    private float TriplerShotTime = -1;
    [SerializeField]
    private float FireRate = 0.5f;
    [SerializeField]
    private int PlayerHealth = 3;

    SpawnManager spawnManager;

    [SerializeField]
    private bool IsTripleShotActive;

    void Start()
    {
        GameObject FindSpawnManager = GameObject.Find("Spawn Manager");
        if(FindSpawnManager != null)
        {
            spawnManager = FindSpawnManager.GetComponent<SpawnManager>();
        } else
        {
            Debug.LogError("Spawn Manager null reference");
        }
    }
    void Update()
    {
        Fire();
        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");
        Boundaries();
        transform.Translate(new Vector3(HorizontalInput, VerticalInput, 0)* Speed * Time.deltaTime);
    }
    void Fire()
    {
        if (Input.GetKey(KeyCode.Space ) && Time.time >= CanFire)
        {
            CanFire = Time.time + FireRate;
            if(IsTripleShotActive == true && Time.time < TriplerShotTime)
            {
                Instantiate(TripleShot, new Vector3(transform.position.x, transform.position.y + BulletOffSet, transform.position.z), Quaternion.identity);
            } else
            {
                Instantiate(LaserPrefab, new Vector3(transform.position.x, transform.position.y + BulletOffSet, transform.position.z), Quaternion.identity);
            }
            

        }
        
    }
    
     void Boundaries()
    {
    
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f,0), 0);

        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        } else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
    }
   public void Damage(int dmg)
    {
        PlayerHealth-=dmg;
        if(PlayerHealth < 1)
        {
            spawnManager.OnPlayerDeath();
            Destroy(gameObject);
        }
    }

    public void TripleShotActive()
    {
        if(IsTripleShotActive == true)
        {
            TriplerShotTime += 5;
        } else
        {
            IsTripleShotActive = true;
            TriplerShotTime = Time.time + 5;
        }
    }
}
