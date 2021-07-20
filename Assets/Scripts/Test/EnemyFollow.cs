using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{

    FollowBehaviorScript FBS;
    [SerializeField]
    private GameObject FollowBehaviorGameObject;
    [SerializeField]
    private int NumberInRow;

    private void Start()
    {
        FBS = FollowBehaviorGameObject.GetComponent<FollowBehaviorScript>();
        for (int x = 0; x < FBS.EShips.Count; x++)
        {
            if (FBS.EShips[x] == this.gameObject)
            {
                print(this.gameObject + " was found in " + FBS.EShips[x]);
                NumberInRow = x;
                return;
            }
        }
    }
    private void Update()
    {
        Kill();
    }
    void Kill()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (NumberInRow == 1)
            {
                FBS.EShips.RemoveAt(0);
                FBS.ReAdjustID();
                Destroy(this.gameObject);
            }
        }
    }
    void FollowShip()
    {

    }
    void Resort()
    {
                for (int x = 0; x < FBS.EShips.Count; x++)
                {
                    if (FBS.EShips[x] == this.gameObject)
                    {
                        print(this.gameObject + " was found in " + FBS.EShips[x]);
                        NumberInRow = x;
                        return;
                    }
                }
            
        
    }
}
