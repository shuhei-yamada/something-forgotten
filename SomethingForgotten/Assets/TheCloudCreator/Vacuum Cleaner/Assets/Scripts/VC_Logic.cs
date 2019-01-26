using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VC_Logic : MonoBehaviour
{
	//	Audio object holders and their Audio source components
	private GameObject vacuumStartObject;
	private AudioSource vacuumStartAudio;
	private GameObject vacuumStopObject;
	private AudioSource vacuumStopAudio;
	private GameObject vacuumContinuousObject;
	private AudioSource vacuumContinuousAudio;
	private bool assigned = false;
	
	//	Variables used to turn "On" and "Off" the vacuum
	public bool TurnedOn = false;
	//	Used to tell if the Vacuum Cleaner is "On" or "Off"
	private bool turnedOn = false;
	//	Used to check if a coroutine is already changing the state"
	private bool changingState = false;
	
	//	The volume at which the continuous vacuum sound starts
	private float continuousVolume;
	//	The amount of seconds it takes for the continuous vacuum sound to start
	private float continuousVolumeStartTime = 0.3f;
	
	
	private void Awake()
	{
		//	Assign all the sound objects
		vacuumStartObject = transform.Find("Sound: VacuumStart").gameObject; 
		vacuumStopObject = transform.Find("Sound: VacuumStop").gameObject;
		vacuumContinuousObject = transform.Find("Sound: VacuumContinuous").gameObject;
		//	Assign all the sound components
		vacuumStartAudio = vacuumStartObject.GetComponent<AudioSource>();
		vacuumStopAudio = vacuumStopObject.GetComponent<AudioSource>();
		vacuumContinuousAudio = vacuumContinuousObject.GetComponent<AudioSource>();
		//	All objects and components have been assigned
		assigned = true;
		
		//	Assign the volume level
		continuousVolume = vacuumContinuousAudio.volume;
		
		//	Check if the Vacuum Cleaner should start at the beginning of the game
		if(TurnedOn)
		{
			TurnOn();
		}else{
			TurnOff();
		}
		
		
	}
	
	private void OnValidate()
	{
		//	If the Vacuum Cleaner is already turning "On" or "Off" then the values is reset
 		if(changingState)
		{
			if(TurnedOn)
			{
				TurnedOn = false;
			}else{
				TurnedOn = true;
			}
		}
		
		// Needed to prevent an error when the Vacuum Cleaner objects are not set yet.
		if(assigned)
		{
			//	Try to start or stop the Vacuum Cleaner
			if(TurnedOn)
			{
				TurnOn();
			}else{
				TurnOff();
			}			
		}
	}
	
	public void TurnOn()
	{		
		if(!changingState)
		{
			if(!turnedOn)
			{
				changingState = true;
				
				//	Start coroutine to start the Vacuum Cleaner
				StartCoroutine(TurnMotorOn());
			}
		}
	}
	
	public void TurnOff()
	{
		if(!changingState)
		{
			if(turnedOn)
			{
				changingState = true;
				
				//	Start coroutine to stop the Vacuum Cleaner
				StartCoroutine(TurnMotorOff());
			}
		}
	}
	
	private IEnumerator TurnMotorOn()
	{
		//	Start playing the startup sound
		if(!vacuumStartAudio.isPlaying)
		{
			vacuumStartAudio.Play();
			
			//	The loop will stop earlier so the fade-in of the "Continuous" sound overlap a bit
			yield return new WaitForSeconds(vacuumStartAudio.clip.length-continuousVolumeStartTime);
		}
		
		//	Fade-in the "Continuous" sound
		StartCoroutine(TurnContinuousMotorOn());
		
		//	Stop the coroutine
		yield return null;
	}
	
	private IEnumerator TurnMotorOff()
	{
		//	Fade the "Continuous" out
		StartCoroutine(TurnContinuousMotorOff());
		
		//	Start playing the "Stop" sound		
		if(!vacuumStopAudio.isPlaying)
		{
			vacuumStopAudio.Play();
			
			yield return new WaitForSeconds(vacuumStopAudio.clip.length);
		}
		
		//	Set the vacuum's state to "Off"
		turnedOn = false;
		TurnedOn = false;
		changingState = false;
		
		//	Stop the coroutine
		yield return null;
	}
	
	private IEnumerator TurnContinuousMotorOn()
	{
		//	Check if the audio is already playing, if not then start playing
		if(!vacuumContinuousAudio.isPlaying)
		{
			//	Reset the volume
			vacuumContinuousAudio.volume = 0f;
			vacuumContinuousAudio.Play();
		}
		
		//	Gradually increase the volume untill the selected volume is reached
		while(vacuumContinuousAudio.volume < continuousVolume)
		{
			vacuumContinuousAudio.volume += continuousVolume * Time.deltaTime / continuousVolumeStartTime;
			yield return null;
		}
		
		//	Set the Vacuum Cleaner's state to "On"
		turnedOn = true;
		TurnedOn = true;
		changingState = false;
		
		//	Stop the coroutine
		yield return null;
	}
	
	private IEnumerator TurnContinuousMotorOff()
	{
		//	Check if the audio volume is above 0playing, if not then lower it incrementally
		while(vacuumContinuousAudio.volume > 0f)
		{
			//	Lower the volume
			vacuumContinuousAudio.volume -= continuousVolume * Time.deltaTime / continuousVolumeStartTime;
			yield return null;
		}
		//	Stop playing the audio sound
		vacuumContinuousAudio.Stop();

		//	Stop the coroutine
		yield return null;
	}
}