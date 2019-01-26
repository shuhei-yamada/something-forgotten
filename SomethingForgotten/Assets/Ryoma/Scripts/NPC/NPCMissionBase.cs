using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#region NPCMissionENums
public enum NPCMissionType
{
	None,
	Wandering,
	Toothpaste,
	Eat,
	Stylish,
	ChangeClothes,
	Escape,
}

[Flags]
public enum GameMissionProgress
{
	ToDo = 1,
	Doing = 1 << 1,
	Success = 1 << 2,
	Failed = 1 << 3,
}
#endregion NPCMissionEnums


#region IStateMachine
public interface IStateMachine
{
	/// <summary>
	/// Missionが割り振られている間呼び出される
	/// </summary>
	void Update();

	/// <summary>
	/// Missionが開始されたときによばれる
	/// </summary>
	void OnEnable();

	/// <summary>
	/// Missionが開始されたときによばれる
	/// </summary>
	void OnDisable();
}
#endregion IStateMachine



#region NPCMission
[System.Serializable]
public abstract class NPCMissionBase : IStateMachine
{
	public GameMissionProgress progress = GameMissionProgress.ToDo;
	public NPCMissionType missionType;
	/// <summary>
	/// Missionの担当者　(PIC)
	/// </summary>
	protected NPCDataReference dataReference;

	/// <summary>
	/// Missionを作成する
	/// </summary>
	/// <param name="personInCharge">Missionの担当者</param>
	/// <param name="missionType">Missionの種類</param>
	public NPCMissionBase(NPCDataReference dataReference, NPCMissionType missionType)
	{
		this.missionType = missionType;
		this.dataReference = dataReference;
	}


	public abstract void CalculateProgress();
	public virtual void OnEnable()
	{
		//Debug.Log(missionType.ToString() + ".OnEnabele();");
		if ((progress & GameMissionProgress.ToDo) != 0)
		{
			progress = GameMissionProgress.Doing;
		}
	}
	public virtual void OnDisable()
	{
		Debug.Log(missionType.ToString() + ".OnDisabele();");
		if ((progress & GameMissionProgress.Doing) != 0)
		{
			progress = GameMissionProgress.ToDo;
		}
	}
	public abstract void Update();

}
#endregion NPCMission