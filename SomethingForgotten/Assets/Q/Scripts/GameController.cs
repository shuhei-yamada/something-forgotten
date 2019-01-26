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
	public Text GameClearMessage;

	private bool _canStart = true;
	private bool _canRestart = false;

	void Awake()
	{
		mInstance = this;
	}

	void Start()
	{
		GameReady();
	}

	void Update()
	{
		if (_canStart && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Joystick1Button0)))
		{
			GameStart();
			_canStart = false;
		}

		if (_canRestart && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Joystick1Button0)))
		{
			GameRestart();
			_canRestart = false;
		}
	}

	public void GameOver()
	{
		Time.timeScale = 0;
		GameOverPanel.SetActive(true);
		GameOverMessage.gameObject.SetActive(true);
		GameClearMessage.gameObject.SetActive(false);
		_canRestart = true;
	}

	public void GameClear()
	{
		Time.timeScale = 0;
		StartPanel.SetActive(false);
		GameOverPanel.SetActive(true);
		GameOverMessage.gameObject.SetActive(false);
		GameClearMessage.gameObject.SetActive(true);
		_canRestart = true;
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
