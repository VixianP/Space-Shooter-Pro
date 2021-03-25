using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehavior : MonoBehaviour
{   
    [SerializeField]
    private float speed;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, speed, 0) * Time.deltaTime);
        if(transform.position.y > 8)
        {
            Destroy(gameObject);
        }
    }
    
}
