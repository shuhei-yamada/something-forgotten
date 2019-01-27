using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

#pragma warning disable 649
	[SerializeField] private AudioClip _buttonPushAudioClip;
	[SerializeField] private AudioClip _getAudioClip;
	[SerializeField] private AudioClip _clearAudioClip;
	[SerializeField] private AudioClip _gameOverAudioClip;
#pragma warning restore 649

	public enum SeType
	{
		ButtonPush,
		ItemGet,
		GameClear,
		GameOver,
	}

	private AudioSource _audioSource;

	void Start()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	public void PlaySe(SeType type)
	{
		switch (type)
		{
			case SeType.ButtonPush:
				_audioSource.PlayOneShot(_buttonPushAudioClip);
				break;
			case SeType.ItemGet:
				_audioSource.PlayOneShot(_getAudioClip);
				break;
			case SeType.GameClear:
				_audioSource.PlayOneShot(_clearAudioClip);
				break;
			case SeType.GameOver:
				_audioSource.PlayOneShot(_gameOverAudioClip);
				break;
		}
	}
}
