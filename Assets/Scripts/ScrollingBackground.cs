using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField]
    float scrollspeed = 0.2f;
    private bool GameStart;
    [SerializeField]
    private SpawnManager SM;

    // start off screen
    // slow in and slow out (just like the boost)

    private void Start()
    {
        
    }
    void Update()
    {
        Bounds();
        transform.Translate(new Vector3(0, -scrollspeed, 0) * Time.deltaTime);
    }
    void Bounds()
    {
        //before start

        //after start
            if (transform.position.y < -9.15f)
            {
                transform.position = new Vector3(0, 12.38f, 5);
            }
    }
}
