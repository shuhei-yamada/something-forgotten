using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
public class EnemySearch : MonoBehaviour
{
	// Variable ////////////////////////////////////////////////////
	public float movePower = 3f;
	public float maxSpeed = 3f;

	public float rayDistance = 10f;
	protected bool raycastEnabled = true;

	// UseOtherConponents
	[SerializeField] private Rigidbody rigid;

	// AIUpdate
	protected IEnemyUpdate process;

	// Methods ////////////////////////////////////////////////////
	protected virtual void Start()
	{
		if (rigid == null) { rigid = GetComponent<Rigidbody>(); }

		// GetComponent<NavMeshAgent> と 物理演算で動くように設定
		wander.Initialize(transform);

		// 始めはうろつく
		process = wander;
	}

	protected virtual void Update()
	{
		process.UpdateProcess();

		LookupDetection();

		if ( wander.isHuman )
		{
			AnimationUpdate();
		}

	}

	// Characterを物理演算によって移動する
	protected virtual void FixedUpdate()
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

			// 目的地の近く以外では移動方向に向ける。
			if (wander.navMeshAgent.remainingDistance > wander.navMeshAgent.stoppingDistance)
			{
				transform.LookAt(transform.position + wander.navMeshAgent.velocity * rayDistance, Vector3.up);
			}
			else
			{
				// 目的地の近くでは速度を下げて、角度を回転させない。目的地近くではあらぶりやすいため
				rigid.velocity = rigid.velocity / 2f;
				rigid.angularVelocity = Vector3.zero;
			}
		}
		// navMeshAgentの計算上の位置とRigidbodyで動かしている実際の位置が違うので
		// 次のナビメッシュの計算位置をリセットする
		wander.navMeshAgent.nextPosition = transform.position;
	}

	private void LookupDetection()
	{
		// raycastEnabledでチェック有無を決める。犬が骨に夢中になっているとき用
		if ( !raycastEnabled ) { return; }

		// Raycast を飛ばしてチェックする
		bool isHit = IsHitRay(transform.position, transform.forward);

		if (isHit)
		{
			var gameContoller = GameObject.Find("GameController").GetComponent<GameController>();

			gameContoller.GameOver();
		}
	}

	/// <summary>
	/// Rayを飛ばしてプレイヤーに当たったらTrueを返す処理
	/// </summary>
	/// <param name="origin">発射地点</param>
	/// <param name="direction">向き</param>
	/// <returns></returns>
	private bool IsHitRay(Vector3 _origin, Vector3 direction)
	{
		Vector3 origin = _origin;
		// rayを飛ばす原点の高さを1に固定する。
		// キャラによって原点が違う。例えば犬は原点が地面にある
		origin.y = -1f;

		//Rayの作成　　　↓Rayを飛ばす原点　　↓Rayを飛ばす方向
		var ray = new Ray(origin, direction);

		//Rayが当たったオブジェクトの情報を入れる箱
		RaycastHit hit;

		//もしRayにオブジェクトが衝突したら
		//                  ↓Ray  ↓Rayが当たったオブジェクト ↓距離
		if (Physics.Raycast(ray, out hit, rayDistance))
		{
			//Rayが当たったオブジェクトのtagがPlayerだったら
			if (hit.collider.tag == "Player")
			{
				#if UNITY_EDITOR
				Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);
				#endif
				return true;
			}
			#if UNITY_EDITOR
			else
			{
				Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.blue);
			}
		}
		else
		{
			Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.blue);
			#endif
		}
		return false;
	}

	void AnimationUpdate()
	{
		wander.animator.SetFloat(wander.moveSpeedID, wander.navMeshAgent.velocity.magnitude);
	}


	// Inner Class
	protected interface IEnemyUpdate
	{
		void UpdateProcess();
	}

	// jam用に簡易的に作っておく。本来はステートPatternをもっとしっかり組んだ方が良い
#region WanderClass
	[System.Serializable]
	public class Wander : IEnemyUpdate
	{
		public NavMeshAgent navMeshAgent;

		[SerializeField] private float nextPosInterval = 10f;
		private float timeCount = 0;

		[SerializeField] private Transform[] wanderPoint;
		private int pointIndex = 0;

		public Animator animator;
		private int walkingID;
		// humanの歩く
		[HideInInspector] public int moveSpeedID;
		// dog の歩く
		[HideInInspector] public bool isHuman;


		public void UpdateProcess()
		{
			// 目的地に近づいたら歩くアニメーションを止めてインターバルに入る
			if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
			{
				timeCount += Time.deltaTime;
				if ( !isHuman )
				{
					if (animator.GetBool(walkingID)) { animator.SetBool(walkingID, false); }
				}
			}

			// インターバルから抜けて歩き始める
			if (timeCount > nextPosInterval)
			{
				timeCount = 0;
				SetNextPosition();
				if (!isHuman)
				{
					if (!animator.GetBool(walkingID)) { animator.SetBool(walkingID, true); }
				}
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

		public void Initialize(Transform transform)
		{
			animator = transform.GetComponent<Animator>();
			walkingID = Animator.StringToHash("IsWalking");
			moveSpeedID = Animator.StringToHash("MoveSpeed");
			isHuman = animator.isHuman;

			// 犬と人で分岐
			if (isHuman)
			{
				animator.SetBool("Grounded", true);
			}
			else
			{
				animator.SetBool(walkingID, true);
			}

			// RequireComponentしてるので必ずある
			navMeshAgent = transform.GetComponent<NavMeshAgent>();
			if (navMeshAgent == null) { Debug.LogError(transform.name + " はnavMeshAgentがありません"); }
			navMeshAgent.updatePosition = false;
			navMeshAgent.updateRotation = false;
			navMeshAgent.updateUpAxis = false;

			bool isSet = navMeshAgent.SetDestination(wanderPoint[0].position);
		}
	}
	[SerializeField] protected Wander wander;
#endregion Wander
}
