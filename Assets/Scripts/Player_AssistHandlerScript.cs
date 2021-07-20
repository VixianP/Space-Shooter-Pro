using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AssistHandlerScript : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> AssistGameObject = new List<GameObject>();
    [SerializeField]
    private GameObject Player;
    private GameObject TempTarget;
    Player_Assist PARetarget;
    private List<GameObject> ListOfAssist = new List<GameObject>();
    private int AssistCount;

    public void SpawnAssist()
    {
        AssistCount = ListOfAssist.Count;
        GameObject NewAssist = Instantiate(AssistGameObject[0], Player.transform.position, Quaternion.identity);
        ListOfAssist.Add(NewAssist);
        PARetarget = NewAssist.GetComponent<Player_Assist>();
        if (AssistCount == 0)
        {
            PARetarget.Retarget(Player);
        } else if (AssistCount > 0)
        {
            TempTarget = ListOfAssist[AssistCount - 1];
            PARetarget.Retarget(TempTarget);
        }
        //call from player function when power up is picked up
        //call from player class PlayerAssistclass
        //instantiate new assist class into parent
        //add to list
    }
}
