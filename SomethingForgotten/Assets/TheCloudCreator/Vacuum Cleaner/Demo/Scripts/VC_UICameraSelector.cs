using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VC_UICameraSelector : MonoBehaviour
{
	//	Value used to store the currently selected camera
	private int currentCamera;
	
	//	Array of cameras that can be selected
	public Camera[] selectableCameras;
	
	private void Start()
	{
		//	Turn off all cameras
		for(int i = 0; i < selectableCameras.Length; i++)
		{
			selectableCameras[i].enabled = false;
			selectableCameras[i].gameObject.SetActive(false);
		}
		
		//	Turn on the first camera
		currentCamera = selectableCameras.Length-1;
		SelectNextCamera();
	}
	
	public void SelectNextCamera()
	{
		//	Have the currentCamera int cycle between 0 and the amount of camera's in the array
		if(currentCamera == selectableCameras.Length-1){
			currentCamera = 0;
		}else{
			currentCamera++;
		}
		
		//	Turn the selected camera on and the other cameras off
		for(int i = 0; i < selectableCameras.Length; i++)
		{
			if(currentCamera == i)
			{
				selectableCameras[i].gameObject.SetActive(true);
				selectableCameras[i].enabled = true;
			}else{
				selectableCameras[i].gameObject.SetActive(false);
				selectableCameras[i].enabled = false;
			}
		}
	}
}