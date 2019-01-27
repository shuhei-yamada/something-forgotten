using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

#pragma warning disable 649
	[SerializeField] private AudioSource _audioSource1;
	[SerializeField] private AudioSource _audioSource2;
	[SerializeField] private AudioClip _buttonPushAudioClip;
	[SerializeField] private AudioClip _forgottenObjectGetAudioClip;
	[SerializeField] private AudioClip _itemGetAudioClip;
	[SerializeField] private AudioClip _clearAudioClip;
	[SerializeField] private AudioClip _gameOverAudioClip;
#pragma warning restore 649

	public enum SeType
	{
		ButtonPush,
		ForgottenObjectGet,
		ItemGet,
		GameClear,
		GameOver,
	}

	public void PlaySe(SeType type)
	{
		switch (type)
		{
			case SeType.ButtonPush:
				_audioSource1.PlayOneShot(_buttonPushAudioClip);
				break;
			case SeType.ForgottenObjectGet:
				_audioSource1.PlayOneShot(_forgottenObjectGetAudioClip);
				break;
			case SeType.ItemGet:
				_audioSource1.PlayOneShot(_itemGetAudioClip);
				break;
			case SeType.GameClear:
				_audioSource2.PlayOneShot(_clearAudioClip);
				break;
			case SeType.GameOver:
				_audioSource2.PlayOneShot(_gameOverAudioClip);
				break;
		}
	}
}
