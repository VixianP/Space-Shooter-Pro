using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    public Text Score;
    [SerializeField]
    Image LivesImage;
    [SerializeField]
    private Sprite[] LivesSprite;
    [SerializeField]
    GameObject GameOverText;
    [SerializeField]
    GameObject RestartText;
    [SerializeField]
    GameObject QuitText;

    private void Start()
    {
        Score.text = "Score :" + 0;
    }

    public void UpdateScore(int Points)
    {
        int CurrentScore = Points;
        Score.text = "Score :" + CurrentScore;
        
    }
    public void UpdateLives(int CurrentLives)
    {
        if(CurrentLives > 0) 
        { 
        LivesImage.sprite = LivesSprite[CurrentLives];
        } else
        {
            LivesImage.sprite = LivesSprite[0];
        }
    }
    public void GameOver()
    {
        GameOverText.SetActive(true);
        RestartText.SetActive(true);
        QuitText.SetActive(true);
    }
}
