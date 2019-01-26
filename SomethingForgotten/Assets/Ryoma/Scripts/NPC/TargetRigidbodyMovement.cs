using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRigidbodyMovement : MonoBehaviour {
	#region Variable
	[SerializeField] private NPCController m_NPCController;
	[SerializeField] private Vector3 moveDirection;
	[SerializeField] private Vector3 moveBeforeDirection;

	[SerializeField] private float speed = 5f;
	[SerializeField] private float maxSpeed = 5f;
	//[SerializeField] private float anglerSpeed = 100;
	Vector3 rigVelBuf;

	[SerializeField] private float drag;

	#endregion Variable

	#region PublicMethod

	#endregion PublicMethod
	#region PrivateMethod

	// Use this for initialization
	private void Start () {
		m_NPCController.dataReference.navMeshAgent.updatePosition = false;
		m_NPCController.dataReference.navMeshAgent.updateRotation = false;
		m_NPCController.dataReference.navMeshAgent.updateUpAxis = false;
		drag = m_NPCController.dataReference.rigidbody.drag;
		speed = m_NPCController.dataReference.navMeshAgent.speed;
		//anglerSpeed = m_NPCController.NPCDataReference.navMeshAgent.angularSpeed;
	}

	
	// Update is called once per frame
	private void FixedUpdate()
	{
		// 気絶中は操作しない
		if (m_NPCController.dataReference.characterReaction.GetCharacterStatus() == CharacterStatus.Collapse) { return; }

		if (m_NPCController.dataReference.navMeshAgent.enabled)
		{
			// 移動
			//moveBeforeDirection = moveDirection;
			moveDirection = speed * (m_NPCController.dataReference.navMeshAgent.velocity);
						
			Vector3 rigVel = m_NPCController.dataReference.rigidbody.velocity;

			Vector3 accelerationDelta = moveDirection * Time.fixedDeltaTime;
			// 抵抗による変位
			float accelerationForDrag = (float)(1.0f - drag * Time.fixedDeltaTime);

			rigVel = ((rigVel + accelerationDelta) * accelerationForDrag);
			// 最大速度より早かったら制限する
			if( rigVel.sqrMagnitude > maxSpeed * maxSpeed)
			{
				rigVel = rigVel.normalized * maxSpeed;
			}
			m_NPCController.dataReference.rigidbody.velocity = rigVel;

			if (m_NPCController.dataReference.navMeshAgent.remainingDistance > m_NPCController.dataReference.navMeshAgent.stoppingDistance)
			{
				m_NPCController.transform.LookAt(transform.position + m_NPCController.dataReference.navMeshAgent.velocity * 100, Vector3.up);
			}
		}
		// navMeshAgentの計算上の位置とRigidbodyで動かしている実際の位置が違うので
		// 次のナビメッシュの計算位置をリセットする
		m_NPCController.dataReference.navMeshAgent.nextPosition = transform.position;
	}

	private void Update()
	{

		//moveDirection = (m_NPCController.NPCDataReference.navMeshAgent.nextPosition - transform.position);
		//moveDirection = m_NPCController.NPCDataReference.navMeshAgent.velocity;
		//Debug.Log("move : " + moveDirection);

	}

	#endregion PrivateMethod
}
