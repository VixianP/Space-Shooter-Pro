using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    [SerializeField]
    private GameObject Target;
    private float SmoothTime = 0.5f;
    private float MaxMovementSpeed = 45;
    Vector2 CurrentVelocity;
    private float MaxDistanceFar = 3, MinDistanceFar = 1;
    private float MaxDistanceClose = 0.5f, MinDistanceClose = 1.5f;
    [SerializeField]
    private int IndexID;

    [SerializeField]
    private bool IsRooted;
    [SerializeField]
    private Vector2 TentacleExtendDirection;

    bool IsTheLeader;

    [SerializeField]
    private Vector2 LeaderMovementPosition;

    [SerializeField]
    private bool IsLeftAndRight;
    [SerializeField]
    private float LeftAndRight;
    [SerializeField]
    private bool IsUpAndDown;
    [SerializeField]
    private float UpAndDown;

    private float X, Y;

    private void Start()
    {
        X = LeftAndRight;
        Y = UpAndDown;
    }

    TentacleHost TentacleHostScript;
    private void Update()
    {
      if(IsTheLeader == false)
        {
            Move();
        }
        else
        {

        }
    }
    public void Move()
    {
        //modfiy every 3rd
        if (Target != null)
        {

            if (Vector2.Distance(transform.position, Target.transform.position) > Random.Range(MinDistanceFar, MaxDistanceFar))
            {
                //transform.position = Vector2.MoveTowards(transform.position, Target.transform.position, 0.05f);
                transform.position = Vector2.SmoothDamp(transform.position, Target.transform.position, ref CurrentVelocity, SmoothTime, MaxMovementSpeed);
            }
            /*else if (Vector2.Distance(transform.position, Target.transform.position) < Random.Range(MinDistanceClose, MaxDistanceClose))
            {
                //transform.position = Vector2.MoveTowards(transform.position, Target.transform.position, -0.05f);
                transform.position = Vector2.SmoothDamp(transform.position, Target.transform.position, ref CurrentVelocity, SmoothTime, -MaxMovementSpeed * 2);
            }*/
        }
    }
    void LeaderMovemment()
    {
        if (IsLeftAndRight==true)
        {
            //pingpong left and right
        }
        if (IsUpAndDown==true)
        {
            //pingpong up and down
        }
        transform.position = LeaderMovementPosition * Time.deltaTime;
    }
    public void SetID(int IDNumber,GameObject TentacleHostGameObject)
    {
        IndexID = IDNumber;
        TentacleHostScript = TentacleHostGameObject.GetComponent<TentacleHost>();
        /*if (IndexID % 2 == 0)
        {
            
            MaxMovementSpeed -= 10;
            SmoothTime = 0.6f;
            print("Im a even number!" + " My index is " + IndexID);
        }*/
    if(IndexID == TentacleHostScript.MaxTetacleLength)
        {
            print("Im the last in line! " + " My index is " + IndexID);
        }
    }
    public void SetTarget(GameObject NewTargetToFollow)
    {
        Target = NewTargetToFollow;
    }
    public void ExtendTentacle()
    {//initial spawning behavior
        if (IsTheLeader)
        {
            
        }
    }
    public void Attack()
    {

    }
}





