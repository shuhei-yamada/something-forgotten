using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VC_ScreenShooter : MonoBehaviour
{
	//	Value used to store the force by which the object is shot
	public int shootingForce = 100;
	//	Value used to store the mass of the object that is being shot
	public float ballMass = 0.3f;
	//	Value used to store all possible primitives that can be shot
	public enum SelectiblePrimitives {Sphere, Capsule, Cube, Cylinder};
	//	Value used to select the primitive that needs to be shot
	public SelectiblePrimitives selectedPrimitive;
	//	Value used to store the primitive
	private PrimitiveType bulletType;
	
	void Update ()
	{
        if (Input.GetMouseButtonDown (0))
        {
			//	Check if the mouse is not on an UI object
			if (!EventSystem.current.IsPointerOverGameObject())
			{
				SelectPrimitive();
				
				//	Create a bullet object of the selected type
				GameObject  bullet = GameObject.CreatePrimitive(bulletType);
				bullet.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
				
				//	Position the bullet at the camera's transform
				bullet.transform.position = Camera.main.transform.position;
				
				//	Add a rigidbody object for collision detection
				Rigidbody rigidbody = bullet.AddComponent<Rigidbody>();
				rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
				rigidbody.mass = ballMass;
				rigidbody.drag = 0.05f;
				rigidbody.angularDrag = 0.05f;
				
				//	Add force to the bullet in the direction to where the user clicks
				Vector3 direction = new Vector3(Input.mousePosition.x,Input.mousePosition.y, 1.0f);
				bullet.transform.LookAt(Camera.main.ScreenToWorldPoint(direction));
				rigidbody.AddRelativeForce(Vector3.forward * shootingForce);
				
				//	Set a timer which will destroy the object
				StartCoroutine(destroyObjectAfterTimer(bullet, 5f));
			}
        }
    }
	
	//	Function used to destroy objects after an amount of seconds
	private IEnumerator destroyObjectAfterTimer(GameObject targetObject, float seconds)
	{
		yield return new WaitForSeconds(seconds);
		
		Destroy(targetObject);
	}
	
	//	Function used to assign the right primitive
	private void SelectPrimitive()
	{
		if(selectedPrimitive == SelectiblePrimitives.Sphere)
		{
			bulletType = PrimitiveType.Sphere;
		}else{
			if(selectedPrimitive == SelectiblePrimitives.Capsule)
			{
				bulletType = PrimitiveType.Capsule;
			}else{
				if(selectedPrimitive == SelectiblePrimitives.Cube)
				{
					bulletType = PrimitiveType.Cube;
				}else{
					if(selectedPrimitive == SelectiblePrimitives.Cylinder)
					{
						bulletType = PrimitiveType.Cylinder;
					}
				}
			}
		}
	}
}
