using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum CharacterStatus
{
	Normal,
	Collapse, // 気絶
}

[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class CharacterReaction : MonoBehaviour {

	[SerializeField] private float m_CollapseThreshold = 4f;

	private Rigidbody m_CharaRigidbody;


	[SerializeField] private CharacterStatus m_CharacterStatus = CharacterStatus.Normal;

	private Coroutine recoverCo;
	[SerializeField] private float m_RecoverTime = 6f;


	#region PublicMethod
	public CharacterStatus GetCharacterStatus()
	{
		return m_CharacterStatus;
	}

	#endregion PublicMethod

	#region PrivateMethod

	// Use this for initialization
	private void Start () {
		m_CharaRigidbody = GetComponent<Rigidbody>();

	}


	private void OnCollisionEnter(Collision otherCollision)
	{
		if (otherCollision.collider.tag == "CantGrabObj") { return; }
		float hitMagnitude = otherCollision.relativeVelocity.magnitude;
		//Debug.Log("hit : " + otherCollision.gameObject.name.ToString() + ", magnitude : " + hitMagnitude);
		if (hitMagnitude > m_CollapseThreshold)
		{
			Collapsed();
			Debug.Log(gameObject.name.ToString() + "は気絶した！" + "\n " + otherCollision.gameObject.name + "にぶつかった : " + hitMagnitude);
			// m_CharaRigidbody.AddForce(otherCollision.rigidbody.velocity * otherCollision.rigidbody.mass * 100, ForceMode.Impulse);
		}
	}

	/// <summary>
	/// 気絶する
	/// </summary>
	private void Collapsed()
	{
		//m_CharacterNavigation.SetNavigationFlag(false);

		//m_CharaRigidbody.isKinematic = false;
		m_CharaRigidbody.constraints = RigidbodyConstraints.None;

		if(m_CharacterStatus == CharacterStatus.Collapse)
		{
			if (recoverCo != null)
			{
				StopCoroutine(recoverCo);
			}
			
		}
		recoverCo = StartCoroutine(CoStartRecoverTimer());


		m_CharacterStatus = CharacterStatus.Collapse;
	}

	private IEnumerator CoStartRecoverTimer()
	{
		yield return new WaitForSeconds(m_RecoverTime);

		Recover();
	}

	/// <summary>
	/// 気絶から回復する
	/// </summary>
	private void Recover()
	{
		// m_CharacterNavigation.SetNavigationFlag(true);

		// m_CharaRigidbody.isKinematic = true;
		transform.eulerAngles = Vector3.zero;
		m_CharaRigidbody.constraints =  RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		transform.eulerAngles = Vector3.zero;

		m_CharacterStatus = CharacterStatus.Normal;
		Debug.Log(gameObject.name.ToString() + "は回復した!! threadID:" + Thread.CurrentThread.ManagedThreadId);
	}

	#endregion PrivateMethod
}
