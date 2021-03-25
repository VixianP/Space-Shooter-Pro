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
    private float BulletOffSet;

    [SerializeField]
    private float CanFire = -1;
    [SerializeField]
    private float FireRate = 0.5f;

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
        if (Input.GetKeyDown(KeyCode.Space ) && Time.time >= CanFire)
        {
            CanFire = Time.time + FireRate;
            Instantiate(LaserPrefab, new Vector3(transform.position.x,transform.position.y + BulletOffSet, transform.position.z), Quaternion.identity);
        }
    }

    public void Boundaries()
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
}
