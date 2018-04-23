using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public int Score;
    public float timeLeft;
    public int startingTime;
    public Text text;
    public Text time;
    public GameObject victoryPanel;
    public Text victoryScore;
    public Text victoryTime;
    public GameObject defeatPanel;
    public Text defeatScore;

    void Start()
    {
        instance = this;
        timeLeft = startingTime;
    }

    public void Update()
    {
        text.text = Score.ToString();
        time.text = timeLeft.ToString("0.00");
        timeLeft -= Time.deltaTime;
        if(timeLeft < 0)
        {
            timeLeft = 0;
            Lose();
        }
    }

    public void AddScore(int value)
    {
        Score += value;
    }

    public void Lose()
    {
        Time.timeScale = 0;
        defeatPanel.SetActive(true);
        defeatScore.text = Score.ToString();
    }

    public void Win()
    {
        Time.timeScale = 0;
        AddScore((int)timeLeft * 2);
        victoryPanel.SetActive(true);
        victoryScore.text = Score.ToString();
        victoryTime.text = timeLeft.ToString("0.00");

    }
    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}
