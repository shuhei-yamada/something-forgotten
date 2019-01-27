using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
public class DogSearcher : EnemySearch
{
	// Variable ////////////////////////////////////////////////////
	WaitForSeconds waitForSeconds = new WaitForSeconds(8f);

	// Methods ////////////////////////////////////////////////////
	protected override void Start()
	{
		base.Start();
		goToBone.Initialize(transform);
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag != "Bone") { return; } // 骨以外では反応しない
		if ( !raycastEnabled ) { return; }		// すでに骨に夢中になっていたら無視

		// raycast をしばらく無効にする
		raycastEnabled = false;
		process = goToBone;
		goToBone.StateEnter(other.transform);

		StartCoroutine(Wandering());
	}

	IEnumerator Wandering()
	{
		yield return waitForSeconds;

		// 元に戻す
		process = wander;
		raycastEnabled = true;
		goToBone.StateExit();
	}

	[System.Serializable]
	public class GoToBone : IEnemyUpdate
	{
		public NavMeshAgent navMeshAgent;
		public Transform bone;

		private Animator animator;
		private int runningID;
		private int walkingID;

		public void Initialize(Transform transform)
		{
			// RequireComponentしてるので必ずある
			navMeshAgent = transform.GetComponent<NavMeshAgent>();
			if (navMeshAgent == null) { Debug.LogError(transform.name + " はnavMeshAgentがありません"); }

			animator = transform.GetComponent<Animator>();
			runningID = Animator.StringToHash("IsRunning");
			walkingID = Animator.StringToHash("IsWalking");
		}

		/// <summary>
		/// processのステートがGotoBoneになった場合の初期化処理
		/// </summary>
		/// <param name="other"></param>
		public void StateEnter(Transform other)
		{
			bone = other;
			animator.SetBool(runningID, true);
			animator.SetBool(walkingID, false);
		}

		/// <summary>
		/// processのステートが終わる際のエンド処理
		/// </summary>
		public void StateExit()
		{
			bone = null;
			animator.SetBool(runningID, false);
			animator.SetBool(walkingID, true);
		}

		public void UpdateProcess()
		{
			if (bone == null) { return; }

			// ToFix:
			// 毎フレームやっていい処理ではなかったと思うので注意。
			// bone が移動したらSetするように変更する時間があればしたい
			navMeshAgent.SetDestination(bone.position);
		}
	}
	[SerializeField] protected GoToBone goToBone;
}