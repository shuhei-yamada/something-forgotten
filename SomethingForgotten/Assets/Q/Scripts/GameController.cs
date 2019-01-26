using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    static GameController mInstance;
    
    public static GameController Instance
    {
        get
        {
            return mInstance;
        }
    }

    public GameObject StartPanel;
    public GameObject GameOverPanel;
    public Text GameOverMessage;

    void Awake()
    {
        mInstance = this;   
    }

    void Start()
    {
        GameReady();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        GameOverPanel.SetActive(true);
        GameOverMessage.text = "GameOver!";
    }

    public void GameClear()
    {
        Time.timeScale = 0;
        StartPanel.SetActive(false);
        GameOverPanel.SetActive(true);
        GameOverMessage.text = "Game Clear!";
    }

    public void GameReady()
    {
        Time.timeScale = 0;
        StartPanel.SetActive(true);
        GameOverPanel.SetActive(false);
    }

    public void GameStart()
    {
        Time.timeScale = 1;
        StartPanel.SetActive(false);
        GameOverPanel.SetActive(false);
    }

    public void GameRestart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
