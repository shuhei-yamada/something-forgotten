using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
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
	public int MaxForGottenObject = 3;
	int ForGottenObject;

	public Text GameClearMessage;
	public Text PressButtonMessage;

	private bool _canStart = true;
	private bool _canRestart = false;
	private bool _isDuringPlay = false;

	private SoundManager _soundManager;

	void Awake()
	{
		mInstance = this;   
	}

	void Start()
	{
		GameReady();
		_soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
	}

	// Update is called once per frame
	void Update()
	{
		if (_canStart && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Joystick1Button0)))
		{
			_soundManager.PlaySe(SoundManager.SeType.ButtonPush);
			GameStart();
			_canStart = false;
		}

		if (_canRestart && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Joystick1Button0)))
		{
			_soundManager.PlaySe(SoundManager.SeType.ButtonPush);
			GameRestart();
			_canRestart = false;
		}
	}

	public void GameOver()
	{
		if (!_isDuringPlay)
		{
			return;
		}

		Debug.Log("GameOver");

		_isDuringPlay = false;
		_soundManager.PlaySe(SoundManager.SeType.GameOver);
		Time.timeScale = 0;
		GameOverPanel.SetActive(true);
		GameOverMessage.gameObject.SetActive(true);
		GameClearMessage.gameObject.SetActive(false);
		Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(_ =>
		{
			PressButtonMessage.gameObject.SetActive(true);
			_canRestart = true;
		});
	}

	public void GameClear()
	{
		_soundManager.PlaySe(SoundManager.SeType.GameClear);
		Time.timeScale = 0;
		StartPanel.SetActive(false);
		GameOverPanel.SetActive(true);
		GameOverMessage.gameObject.SetActive(false);
		GameClearMessage.gameObject.SetActive(true);
		Observable.Timer(TimeSpan.FromSeconds(1)).TakeUntilDestroy(this).Subscribe(_ =>
		{
			PressButtonMessage.gameObject.SetActive(true);
			_canRestart = true;
		});
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
		_isDuringPlay = true;
	}

	public void GameRestart()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	public void GameExit()
	{
		Application.Quit();
	}

	public void PlaySe(SoundManager.SeType seType)
	{
		_soundManager.PlaySe(seType);
	}
}
