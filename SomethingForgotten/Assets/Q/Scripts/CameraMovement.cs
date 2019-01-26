using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public Transform Target;
	public float SmoothSpeed = 5f;
	Vector3 NewPos;
	Vector3 Offset;	

    // Start is called before the first frame update
    void Start()
	{
		Offset = transform.position - Target.position;
    }

    // Update is called once per frame
    void Update()
    {
		NewPos = Target.position + Offset;
		transform.position = Vector3.Lerp(transform.position,NewPos,SmoothSpeed*Time.deltaTime);
    }
}
