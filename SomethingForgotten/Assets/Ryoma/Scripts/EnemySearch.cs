using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
public class EnemySearch : MonoBehaviour
{
	// Variable ////////////////////////////////////////////
	public float movePower = 3f;
	public float maxSpeed = 3f;


	// UseOtherConponents
	[SerializeField] private Rigidbody rigid;

	// AIUpdate
	IEnemyUpdate process;




	// Methods /////////////////////////////////////////////

	private void Start()
	{
		if (wander.navMeshAgent == null) { wander.navMeshAgent = GetComponent<NavMeshAgent>(); }
		if (rigid == null) { rigid = GetComponent<Rigidbody>(); }

		SetupNavMeshAgent();

		// 始めはうろつく
		process = wander;
	}

	private void Update()
	{
		process.UpdateProcess();
	}

	// Characterを物理演算によって移動する
	private void FixedUpdate()
	{

		if (wander.navMeshAgent.enabled)
		{
			//moveBeforeDirection = moveDirection;
			Vector3 navMeshAgentVelocity = wander.navMeshAgent.velocity;
			Vector3 moveDirection = movePower * (navMeshAgentVelocity);

			Vector3 rigVel = rigid.velocity;

			Vector3 accelerationDelta = moveDirection * Time.fixedDeltaTime;
			// 抵抗による変位
			float accelerationForDrag = (float)(1.0f - rigid.drag * Time.fixedDeltaTime);

			rigVel = ((rigVel + accelerationDelta) * accelerationForDrag);
			// 最大速度より早かったら制限する
			if (rigVel.sqrMagnitude > maxSpeed * maxSpeed)
			{
				rigVel = rigVel.normalized * maxSpeed;
			}
			rigid.velocity = rigVel;

			if (wander.navMeshAgent.remainingDistance > wander.navMeshAgent.stoppingDistance)
			{
				transform.LookAt(transform.position + wander.navMeshAgent.velocity * 100, Vector3.up);
			}
		}
		// navMeshAgentの計算上の位置とRigidbodyで動かしている実際の位置が違うので
		// 次のナビメッシュの計算位置をリセットする
		wander.navMeshAgent.nextPosition = transform.position;
	}

	private void SetupNavMeshAgent()
	{
		wander.navMeshAgent.updatePosition = false;
		wander.navMeshAgent.updateRotation = false;
		wander.navMeshAgent.updateUpAxis = false;
	}


	// Inner Class
	interface IEnemyUpdate
	{
		void UpdateProcess();
	}


	#region WanderClass
	[System.Serializable]
	public class Wander : IEnemyUpdate
	{
		public NavMeshAgent navMeshAgent;

		[SerializeField] private float nextPosInterval = 10f;
		private float timeCount = 0;

		[SerializeField] private Transform[] wanderPoint;
		private int pointIndex = 0;


		public void UpdateProcess()
		{
			timeCount += Time.deltaTime;
			if (timeCount >= nextPosInterval)
			{
				timeCount = 0;
				SetNextPosition();
			}
		}

		public void SetNextPosition()
		{
			pointIndex++;
			if (wanderPoint.Length <= pointIndex)
			{
				pointIndex = 0;
			}

			// エージェントがナビメッシュ上にいる場合、行先を設定
			if (navMeshAgent.isOnNavMesh)
			{
				
				bool isSet = navMeshAgent.SetDestination(wanderPoint[pointIndex].position);
				if (!isSet) { Debug.LogError(navMeshAgent.transform.name + " はナビメッシュError:目的地が設定できませんでした.\nWanderPointを確認してください"); }
			}
			else
			{
				Debug.Log(navMeshAgent.transform.name + " は今NavMesh上にいません");
			}
		}
	}
	[SerializeField] private Wander wander;
	#endregion Wander
}
