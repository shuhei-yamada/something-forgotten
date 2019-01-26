using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;



public class NPCController : MonoBehaviour {
	#region Variable

	[SerializeField] private NPCMissionBase m_CurrentMission;
	[SerializeField] private List<NPCMissionBase> m_MissionList = new List<NPCMissionBase>();
	private int missionIndex = 0;

	[SerializeField] private TargetRigidbodyMovement m_TargetRigidbodyMovement;

	[Header("うろつく(Wandering)")]
	[Tooltip("場所を変更する時間の間隔")]
	[SerializeField] private int wanderInterval = 10;

	[SerializeField] private Transform[] m_WanderPositions;

	//[SerializeField]
	private NPCDataReference m_DataReference;
	public NPCDataReference dataReference { get { return m_DataReference; } private set { m_DataReference = value; } }

	#endregion Variable

	#region PublicMethod
	
	public NPCMissionBase NextMission(int increment = 1)
	{
		// 前のMissionの終了処理
		m_CurrentMission.CalculateProgress();
		m_CurrentMission.OnDisable();

		missionIndex += increment;

		// 新しいミッションの開始処理
		m_CurrentMission = m_MissionList[missionIndex];
		m_CurrentMission.OnEnable();
		return m_CurrentMission;
	}

	public int GetAllMssionNum()
	{
		return m_MissionList.Count;
	}

	#endregion PublicMethod

	#region PrivateMethod

	private void Awake()
	{
		dataReference = new NPCDataReference(
			gameObject,
			gameObject.transform,
			gameObject.GetComponent<NavMeshAgent>(),
			gameObject.GetComponent<Rigidbody>(),
			gameObject.GetComponent<CharacterReaction>(),
			m_WanderPositions.Select(v => v.position).ToArray(),
			wanderInterval
		);
	}

	// Use this for initialization
	private void Start()
	{
		m_MissionList.Add(new Wander(dataReference));

		m_CurrentMission = m_MissionList[missionIndex];
		m_CurrentMission.OnEnable();

	}

	// Update is called once per frame
	private void Update () {
		m_CurrentMission.Update();
		//Debug.Log("maxAngleVel"+ NPCDataReference.rigidbody.maxAngularVelocity);
		//Debug.Log("maxDepentralVel" + NPCDataReference.rigidbody.maxDepenetrationVelocity);
	}

	private void OnDestroy()
	{
		m_CurrentMission = null;
		m_MissionList = null;
		dataReference = null;
	}
	#endregion PrivateMethod
}
