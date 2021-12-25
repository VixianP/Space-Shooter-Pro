using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundActor : MonoBehaviour
{
    [SerializeField][Range(0,3)]
    private int Mode;
    [SerializeField]
    private bool[] SequenceBooleans;
    [SerializeField][Range(0, 6)]
    private float[] Timer;

    [SerializeField]
    private float RotationSpeed;
    [SerializeField]
    private float LeftAndRightRotationRange;

    [SerializeField]
    private float Velocity;

    void Update()
    {
        Sequencer();
    }
    void Sequencer()
    {
        if (Timer != null)
        {
            if (Time.time > Timer[Mode] + Time.time && Timer[Mode] != 0)
            {
                if (SequenceBooleans != null)
                {
                    SequenceBooleans[Mode] = false;
                }
                else
                {
                    Debug.LogError("Background Actor _ Nothing inside Sequence boolean");
                }
            }
        }
        LeftAndRightRotation();
        ConstantRotation();
        ConstantVelocity();
    }
    void LeftAndRightRotation() //0
    {
        if (SequenceBooleans[0] == true)
        {
            if (transform.rotation.z >= LeftAndRightRotationRange)
            {
                transform.Rotate(new Vector3(0, 0, -RotationSpeed * Time.deltaTime));
            }
            else if (transform.rotation.z < LeftAndRightRotationRange)
            {
                transform.Rotate(new Vector3(0, 0, RotationSpeed * Time.deltaTime));
            }
        }
    }
    void ConstantRotation() //1
    {
        if (SequenceBooleans[1] == true && SequenceBooleans[0] == false)
        {
            transform.Rotate(new Vector3(0, 0, RotationSpeed * Time.deltaTime));
        }
        else
        {
            SequenceBooleans[0] = false;
        }
    }
    void ConstantVelocity() //2
    {
        if (SequenceBooleans[2] == true)
        {
            float Y = +Velocity;
            transform.position = new Vector3(0, Y * Time.deltaTime, 0);
        }
    }
}
