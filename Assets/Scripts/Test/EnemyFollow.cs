using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    private Vector2 Direction = new Vector2(0,-1);

    bool isDead = false;
    bool isTurning = false;
    bool LeftRight = false;

    float MovementTimer = -1;

    //set 5 in a row and adjust values accordingly. this will be a seperate group of spawned enemies
    //int turntime
    //int forward time

    //two phase, following and not following

    //first in list and last. if last in list then follow one index below

    // Update is called once per frame
    void Update()
    {
        Movement();
        transform.Translate(Direction * Time.deltaTime);
    }
    
    void Movement()
    {
        if(Time.time > MovementTimer)
        {
            if(isTurning == false)
            {
                Direction = new Vector2(0, -1);
                isTurning = true;
                MovementTimer = Time.time + 4;
            } else if(isTurning == true)
            {
                if(LeftRight == false)
                {
                    Direction = new Vector2(-1, 0);
                    LeftRight = !LeftRight;
                    isTurning = false;
                    MovementTimer = Time.time + 3;
                } else if(LeftRight == true)
                {
                    Direction = new Vector2(1, 0);
                    LeftRight = !LeftRight;
                    isTurning = false;
                    MovementTimer = Time.time + 3;
                }
            }
        }
    }

}
