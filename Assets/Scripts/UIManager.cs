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

    [SerializeField]
    Canvas PlayerUICanvas;
    [SerializeField]
    Canvas PauseMenuUI;
    [SerializeField]
    Slider BoostSlider;

    [SerializeField]
    private Text AmmoCount;

    private void Start()
    {
        Score.text = "Score : " + 0;
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
    public void UpdateAmmo(int Ammo, int ammo2)
    {
        AmmoCount.text = Ammo +"/"+ammo2;
    }
    public void GameOver()
    {
        GameOverText.SetActive(true);
        RestartText.SetActive(true);
        QuitText.SetActive(true);
    }
    public void PauseMenu()
    {
        PauseMenuUI.gameObject.SetActive(true);
        PlayerUICanvas.enabled = false;
        Time.timeScale = 0;
    }
    public void UnPauseMenu()
    {
        PlayerUICanvas.enabled = true;
        PauseMenuUI.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    public void UpdateBoostUI(float boost,int MinMax)//used to round off values
    {
        BoostSlider.value += boost;
        if(MinMax == 1)
        {
            BoostSlider.value = 1;
        } else if(MinMax == 0)
        {
            BoostSlider.value = 0;
        }
       
    }

}
