using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehaviorScript : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> EShips = new List<GameObject>();

    public void ReAdjustID()
    {
        foreach(GameObject ship in EShips)
        {
            ship.GetComponent<EnemyFollow>().Invoke("Resort", 0);
        }
    }
}
