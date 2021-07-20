using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Assist : MonoBehaviour
{
    [SerializeField]
    GameObject Target;
    [SerializeField]
    GameObject ABullet;

    float TimeToShoot = -1;

    // Update is called once per frame
    void Update()
    {
        if(Target != null )
        {
            Fire();
            if (Vector2.Distance(transform.position, Target.transform.position) > 0.9f)
            {
                transform.position = Vector2.MoveTowards(transform.position, Target.transform.position, 0.01f);
            }
        }
    }
    void Fire()
    {
        if (Input.GetKey(KeyCode.A) &  Time.time > TimeToShoot )
        {
            Instantiate(ABullet, transform.position, Quaternion.identity);
            TimeToShoot = Time.time + 0.1f;
        }
    }
    public void Retarget(GameObject NewTarget)
    {
        print("Hit");
        Target = NewTarget;
    }
}
