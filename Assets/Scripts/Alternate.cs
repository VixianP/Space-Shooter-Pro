using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alternate : MonoBehaviour
{
    [SerializeField]
    GameObject Player;
    [SerializeField]
    private float FollowDistance;
    [SerializeField]
    private float MovementSpeed;
    [SerializeField]
    GameObject Laser;

    private float CanFire;
    [SerializeField]
    private float BulletOffSet;
    [SerializeField]
    private float FireRate;

    // Start is called before the first frame update
    void Start()
    {
        GameObject FindPlayer = GameObject.Find("Player");
        if(FindPlayer != null)
        {
            Player = FindPlayer;
        } else
        {
            Debug.LogError("Alternate: Player not Found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position *.80f, FollowDistance);
        Fire();
       
    }
    void Fire()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time >= CanFire)
        {
            CanFire = Time.time + FireRate;
            Instantiate(Laser, new Vector3(transform.position.x, transform.position.y + BulletOffSet, transform.position.z), Quaternion.identity);
            }

        }

    }


