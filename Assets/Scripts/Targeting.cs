using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    [SerializeField]
    GameObject Laser;

    LaserBehavior LaserScript;

    private void Start()
    {
        LaserScript = Laser.GetComponent<LaserBehavior>();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Enemy")
        {
            print("hit");
            LaserScript.ReAssign(coll.gameObject);
        }
    }

}
