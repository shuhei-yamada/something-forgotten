using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationManager : MonoBehaviour
{
	private Animator animator;


	// Start is called before the first frame update
	private void Start()
	{
		animator = GetComponent<Animator>();

	}

}
