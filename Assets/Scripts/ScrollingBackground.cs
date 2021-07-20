using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField]
    float scrollspeed = 0.2f;
    void Update()
    {
        Bounds();
        transform.Translate(new Vector3(0, -scrollspeed, 0) * Time.deltaTime);
    }
    void Bounds()
    {
        if(transform.position.y < -9.15f)
        {
            transform.position = new Vector3(0, 12.38f, 5);
        }
    }
}
