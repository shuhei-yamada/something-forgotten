using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VC_UILogic : MonoBehaviour
{
	//	Used to store the Vacuum Cleaner prefab
	public GameObject vacuum;
	
	//	Set the state of the Vacuum Cleaner to either "On" or "Off"
	public void SetVacuumState(bool value)
	{
		if(value)
		{
			vacuum.GetComponent<VC_Logic>().TurnOn();
		}else{
			vacuum.GetComponent<VC_Logic>().TurnOff();
		}
	}
	
	//	Reset the Vacuum Cleaner to it's original position
	public void ResetScene()
	{
		Rigidbody[] allChildRBs = vacuum.GetComponentsInChildren<Rigidbody>();
		
		//	Stop all motion on the rigid bodies
		for (int i=0; i < allChildRBs.Length; i++)
		{
			allChildRBs[i].angularVelocity = new Vector3(0f,0f,0f);
			allChildRBs[i].velocity = new Vector3(0f,0f,0f);
		}
		
		//	Reset the vacuum cleaner and it's motion
		Rigidbody vacuumRB = vacuum.GetComponent<Rigidbody>();
		vacuumRB.angularVelocity = new Vector3(0f,0f,0f);
		vacuumRB.velocity = new Vector3(0f,0f,0f);
		vacuumRB.MovePosition(new Vector3(0f,0.2f,0f));
		vacuumRB.MoveRotation(Quaternion.Euler(new Vector3(0f,0f,0f)));
	}
}