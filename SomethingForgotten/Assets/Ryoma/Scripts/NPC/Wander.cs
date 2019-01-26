using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

[System.Serializable]
public class Wander : NPCMissionBase
{


	private bool isNavgationThisSecond = false;

	// うろつく場所の配列
	[Tooltip("うろつく場所")]
	private Vector3[] wanderPositions;
	private int wanderPosIndex = 0;


	#region PublicMethod

	public Wander(NPCDataReference dataReference) : base(dataReference, NPCMissionType.Wandering)
	{
		wanderPositions = dataReference.wanderPositions;

		if (wanderPositions.Length <= 1)
		{
			Debug.LogError("Wandering move position.length <= 1");
		}
	}


	/// <summary>
	/// ナビゲーションを止めたり再開したりする
	/// </summary>
	/// <param name="enable"></param>
	public void SetNavigationFlag(bool enable)
	{
		dataReference.navMeshAgent.enabled = enable;

		// Offにするだけならここで終わり
		if (!enable) { return; }
		
		// Onにする場合目的地を付ける
		if (dataReference.navMeshAgent.isOnNavMesh)
		{
			dataReference.navMeshAgent.SetDestination(dataReference.transform.position);
		}
		
	}

	public override void OnEnable()
	{
		base.OnEnable();
		// navMeshAgentがOFFだったらオンにする
		if (!dataReference.navMeshAgent.enabled) { dataReference.navMeshAgent.enabled = true; }
		isNavgationThisSecond = false;
	}

	public override void OnDisable()
	{
		base.OnDisable();
		dataReference.navMeshAgent.enabled = false;
	}


	public override void Update()
	{
		// 行先を決める
		Navigate();
	}

	public override void CalculateProgress()
	{
		// うろちょろするだけなので、失敗の定義がない。全て成功にしておく
		progress = GameMissionProgress.Success;
	}

	#endregion PublicMethod

	#region PrivateMethod

	private void Navigate()
	{
		//Debug.Log("Navigate");
		if (!dataReference.navMeshAgent.enabled) { return; }
		//Debug.Log("wanderInterval" + dataReference.wanderInterval);

		bool isChangeDistination = ((int)Time.time % dataReference.wanderInterval) == 0;
		if (isChangeDistination)
		{
			if (!isNavgationThisSecond)
			{
				isNavgationThisSecond = true;

				//Debug.Log("Change Wandering Pos" + dataReference.playTimer.RemainTimeLimit);
				ChangeDestination();
			}

		}
		else
		{
			isNavgationThisSecond = false;
		}
		
	}

	/// <summary>
	/// 目的地を変更する
	/// </summary>
	private void ChangeDestination()
	{
		if(wanderPositions.Length == 0) { return; }
		int newWanderPosIndex = 0;
		// ランダムで次の目標を設定。ただし、同じ場所を指定している間やり直し
		do
		{
			newWanderPosIndex = Random.Range(0, wanderPositions.Length);
		} while (wanderPosIndex == newWanderPosIndex);
		wanderPosIndex = newWanderPosIndex;

		// エージェントがナビメッシュ上にいる場合、行先を設定
		if (dataReference.navMeshAgent.isOnNavMesh)
		{
			bool isSet = dataReference.navMeshAgent.SetDestination(wanderPositions[wanderPosIndex]);
			if (!isSet) { Debug.LogError("ナビメッシュError:目的地が設定できませんでした" ); }
		}
		else
		{
			Debug.Log((dataReference.transform.name) + " は今NavMesh上にいません");
		}
	}



	#endregion PrivateMethod
}
