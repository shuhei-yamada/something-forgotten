using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private Rigidbody _rigidbody;

	void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	void Update()
	{
	    if (Input.GetKey(KeyCode.W))
	    {
		    var position = _rigidbody.position;
			//position += new Vector3(0, 0, 0.1f);
			position += transform.forward * 0.1f;
			_rigidbody.position = position;
	    }
	    if (Input.GetKey(KeyCode.S))
	    {
		    var position = _rigidbody.position;
		    //position += new Vector3(0, 0, -0.1f);
		    position += transform.forward * -0.1f;
			_rigidbody.position = position;
	    }

	    if (Input.GetKey(KeyCode.A))
	    {
		    //var position = _rigidbody.position;
		    //position += new Vector3(-0.1f, 0, 0);
		    //_rigidbody.position = position;
			transform.Rotate(new Vector3(0, -2, 0));
	    }

		if (Input.GetKey(KeyCode.D))
	    {
		    transform.Rotate(new Vector3(0, 2, 0));
		}

	}
}
