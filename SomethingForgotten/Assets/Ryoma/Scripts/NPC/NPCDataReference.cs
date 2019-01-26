using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class NPCDataReference {
	#region Variable
	public GameObject gameObject { get; private set; }
	public Transform transform { get; private set; }
	public NavMeshAgent navMeshAgent { get; private set; }
	public Rigidbody rigidbody { get; private set; }
	public CharacterReaction characterReaction { get; private set; }
	public Vector3[] wanderPositions { get; private set; }
	public int wanderInterval { get; private set; }
	#endregion Variable

	public NPCDataReference(
		GameObject gameObject,
		Transform transform,
		NavMeshAgent naviMeshAgent,
		Rigidbody rigidbody,
		CharacterReaction characterReaction,
		Vector3[] wanderPositions,
		int wanderInterval
		)
	{
		this.gameObject = gameObject;
		this.transform = transform;
		this.navMeshAgent = naviMeshAgent;
		this.rigidbody = rigidbody;
		this.characterReaction = characterReaction;
		this.wanderPositions = wanderPositions;
		this.wanderInterval = wanderInterval;
	}
}
